// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Pages;

internal sealed partial class WindowWalkerListPage : DynamicListPage, IDisposable
{
	private readonly bool _filterByDesktop;

	public WindowWalkerListPage(string id, string name, string placeholderText, bool filterByDesktop = false)
	{
		Icon = Icons.WalkThisWayIcon;
		Id = id;
		Name = name;
		PlaceholderText = placeholderText;
		_filterByDesktop = filterByDesktop;

		WindowEventHook.Instance.Subscribe(OnWindowsChanged);
		WindowEventHook.Instance.SubscribeTitleChanged(OnTitlesChanged);

		if (_filterByDesktop)
		{
			WindowEventHook.Instance.SubscribeDesktopChanged(OnWindowsChanged);
		}
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
			var windows = OpenWindows.Instance.GetWindows();

			if (_filterByDesktop && SettingsManager.Instance.DockCurrentDesktopOnly)
			{
				windows = windows
					.Where(kv => VirtualDesktopHelper.IsWindowOnCurrentDesktop(kv.Key))
					.ToImmutableDictionary();
			}

			return ResultHelper.GetResultList(windows, SearchText ?? string.Empty);
		}
		catch
		{
			return [];
		}
	}

	private void OnWindowsChanged()
	{
		RaiseItemsChanged(0);
	}

	private void OnTitlesChanged()
	{
		ResultHelper.UpdateCachedTitles(OpenWindows.Instance.GetWindows());
	}

	public void Dispose()
	{
		WindowEventHook.Instance.Unsubscribe(OnWindowsChanged);
		WindowEventHook.Instance.UnsubscribeTitleChanged(OnTitlesChanged);

		if (_filterByDesktop)
		{
			WindowEventHook.Instance.UnsubscribeDesktopChanged(OnWindowsChanged);
		}
	}
}
