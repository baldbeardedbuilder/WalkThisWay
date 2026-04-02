// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Pages;

internal sealed partial class WindowWalkerListPage : DynamicListPage, IDisposable
{
	private readonly Timer _dockRefreshTimer;
	private readonly Timer _titleRefreshTimer;
	private string _lastHwndFingerprint = string.Empty;
	private string _lastTitleFingerprint = string.Empty;

	public WindowWalkerListPage()
	{
		Icon = Icons.WalkThisWayIcon;
		Name = Resources.plugin_name;
		Id = "com.baldbeardedbuilder.cmdpal.walkthisway";
		PlaceholderText = Resources.search_placeholder;

		// Fast timer: detect windows opening/closing every 2 seconds
		_dockRefreshTimer = new Timer(OnDockRefreshTimer, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

		// Slow timer: pick up title changes every 2 minutes
		_titleRefreshTimer = new Timer(OnTitleRefreshTimer, null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));
	}

	public override void UpdateSearchText(string oldSearch, string newSearch)
	{
		RaiseItemsChanged(0);
	}

	public override IListItem[] GetItems()
	{
		try
		{
			OpenWindows.Instance.UpdateOpenWindowsList();
			return ResultHelper.GetResultList(OpenWindows.Instance.GetWindows(), SearchText ?? string.Empty);
		}
		catch
		{
			return [];
		}
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

	public void Dispose()
	{
		_dockRefreshTimer?.Dispose();
		_titleRefreshTimer?.Dispose();
	}
}
