// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Pages;

internal sealed partial class WindowWalkerListPage : DynamicListPage, IDisposable
{
	public WindowWalkerListPage(string id, string name, string placeholderText)
	{
		Icon = Icons.WalkThisWayIcon;
		Id = id;
		Name = name;
		PlaceholderText = placeholderText;

		WindowEventHook.Instance.Subscribe(OnWindowsChanged);
		WindowEventHook.Instance.SubscribeTitleChanged(OnTitlesChanged);
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
	}
}
