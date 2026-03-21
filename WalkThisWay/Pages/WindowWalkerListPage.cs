// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Pages;

internal sealed partial class WindowWalkerListPage : DynamicListPage
{
	public WindowWalkerListPage()
	{
		Icon = Icons.WalkThisWayIcon;
		Name = Resources.plugin_name;
		Id = "com.baldbeardedbuilder.cmdpal.walkthisway";
		PlaceholderText = Resources.search_placeholder;
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
			return ResultHelper.GetResultList(windows, SearchText ?? string.Empty);
		}
		catch
		{
			return [];
		}
	}
}
