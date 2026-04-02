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
	private readonly ICommandItem[] _topLevelItems;
	private readonly ListPage _listPage;
	private readonly CommandItem _item;

	public WalkThisWayCommandsProvider()
	{
		Id = "com.baldbeardedbuilder.cmdpal.walkthisway";
		DisplayName = Resources.plugin_name;
		Icon = Icons.WalkThisWayIcon;
		_listPage = new WindowWalkerListPage();
		_item =
			new CommandItem(_listPage)
			{
				Icon = Icons.WalkThisWayIcon,
				Title = Resources.top_level_command_title,
			};
		_topLevelItems =
		[
			_item
		];
	}

	public override ICommandItem[] TopLevelCommands() => _topLevelItems;

	public override ICommandItem[]? GetDockBands()
	{
		
		//var bandItems = OpenWindowsBand.GetOpenWindowItems();

		//var wrappedBand = new WrappedDockItem(
		//	bandItems,
		//	"com.baldbeardedbuilder.cmdpal.walkthisway.openWindows",
		//	Resources.dock_band_title);

		//wrappedBand.Icon = Icons.WalkThisWayIcon;

		return [_item];
	}

	public override void Dispose()
	{
		base.Dispose();
		GC.SuppressFinalize(this);
	}
}
