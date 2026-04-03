// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CmdPal.Ext.WalkThisWay.Pages;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay;

public sealed partial class WalkThisWayCommandsProvider : CommandProvider
{
	private readonly WindowWalkerListPage _topLevelPage;
	private readonly WindowWalkerListPage _dockPage;
	private readonly ICommandItem[] _topLevelItems;
	private readonly ICommandItem[] _dockBandItems;

	public WalkThisWayCommandsProvider()
	{
		Id = "com.baldbeardedbuilder.cmdpal.walkthisway";
		DisplayName = Resources.plugin_name;
		Icon = Icons.WalkThisWayIcon;

		_topLevelPage = new WindowWalkerListPage(
			"com.baldbeardedbuilder.cmdpal.walkthisway.windows",
			Resources.plugin_name,
			Resources.search_placeholder);

		_dockPage = new WindowWalkerListPage(
			"com.baldbeardedbuilder.cmdpal.walkthisway.windows",
			Resources.dock_band_title,
			Resources.search_placeholder);

		_topLevelItems =
		[
			new CommandItem(_topLevelPage)
			{
				Icon = Icons.WalkThisWayIcon,
				Title = Resources.top_level_command_title,
			},
		];

		_dockBandItems =
		[
			new CommandItem(_dockPage)
			{
				Icon = Icons.WalkThisWayIcon,
				Title = Resources.dock_band_title,
			},
		];
	}

	public override ICommandItem[] TopLevelCommands() => _topLevelItems;

	public override ICommandItem[]? GetDockBands() => _dockBandItems;

	public override void Dispose()
	{
		_topLevelPage.Dispose();
		_dockPage.Dispose();
		WindowEventHook.Instance.Dispose();
		base.Dispose();
		GC.SuppressFinalize(this);
	}
}
