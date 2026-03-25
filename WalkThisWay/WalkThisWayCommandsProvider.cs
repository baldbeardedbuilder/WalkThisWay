// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CmdPal.Ext.WalkThisWay.DockBands;
using Microsoft.CmdPal.Ext.WalkThisWay.Pages;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay;

public sealed partial class WalkThisWayCommandsProvider : CommandProvider
{
	private readonly WindowWalkerListPage _windowWalkerPage;
	private readonly ICommandItem[] _topLevelItems;
	private readonly Timer _dockRefreshTimer;
	private readonly Timer _titleRefreshTimer;
	private string _lastHwndFingerprint = string.Empty;
	private string _lastTitleFingerprint = string.Empty;

	public WalkThisWayCommandsProvider()
	{
		Id = "com.baldbeardedbuilder.cmdpal.walkthisway";
		DisplayName = Resources.plugin_name;
		Icon = Icons.WalkThisWayIcon;

		_windowWalkerPage = new WindowWalkerListPage();

		_topLevelItems =
		[
			new CommandItem(_windowWalkerPage)
			{
				Icon = Icons.WalkThisWayIcon,
				Title = Resources.top_level_command_title,
			},
		];

		// Fast timer: detect windows opening/closing every 2 seconds
		_dockRefreshTimer = new Timer(OnDockRefreshTimer, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

		// Slow timer: pick up title changes every 2 minutes
		_titleRefreshTimer = new Timer(OnTitleRefreshTimer, null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));
	}

	private void OnDockRefreshTimer(object? state)
	{
		try
		{
			OpenWindows.Instance.UpdateOpenWindowsList();

			var fingerprint = BuildHwndFingerprint();
			if (!string.Equals(fingerprint, _lastHwndFingerprint, StringComparison.Ordinal))
			{
				_lastHwndFingerprint = fingerprint;
				_lastTitleFingerprint = BuildTitleFingerprint();
				RaiseItemsChanged(0);
			}
		}
		catch
		{
			// Swallow errors from background refresh
		}
	}

	private void OnTitleRefreshTimer(object? state)
	{
		try
		{
			OpenWindows.Instance.UpdateOpenWindowsList();

			var fingerprint = BuildTitleFingerprint();
			if (!string.Equals(fingerprint, _lastTitleFingerprint, StringComparison.Ordinal))
			{
				_lastTitleFingerprint = fingerprint;
				_lastHwndFingerprint = BuildHwndFingerprint();
				RaiseItemsChanged(0);
			}
		}
		catch
		{
			// Swallow errors from background refresh
		}
	}

	private static string BuildHwndFingerprint()
	{
		var windows = OpenWindows.Instance.GetWindows();
		return string.Join('|', windows.Keys.Select(k => k.ToString(CultureInfo.InvariantCulture)));
	}

	private static string BuildTitleFingerprint()
	{
		var windows = OpenWindows.Instance.GetWindows();
		return string.Join('|', windows.Select(kv => $"{kv.Key.ToString(CultureInfo.InvariantCulture)}:{kv.Value.Title}"));
	}

	public override ICommandItem[] TopLevelCommands() => _topLevelItems;

	public override ICommandItem[]? GetDockBands()
	{
		var bandItems = OpenWindowsBand.GetOpenWindowItems();

		var wrappedBand = new WrappedDockItem(
			bandItems,
			"com.baldbeardedbuilder.cmdpal.walkthisway.openWindows",
			Resources.dock_band_title);

		wrappedBand.Icon = Icons.WalkThisWayIcon;

		return [wrappedBand];
	}

	public override void Dispose()
	{
		_dockRefreshTimer?.Dispose();
		_titleRefreshTimer?.Dispose();
		base.Dispose();
		GC.SuppressFinalize(this);
	}
}
