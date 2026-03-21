// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Commands;
using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.DockBands;

internal static class OpenWindowsBand
{
	internal static IListItem[] GetOpenWindowItems()
	{
		try
		{
			var windows = OpenWindows.Instance.GetWindows();

			return windows.Select(w =>
			{
				var switchCommand = new SwitchToWindowCommand(w);
				return new ListItem(switchCommand)
				{
					Title = w.Title,
					Subtitle = w.ProcessName,
					Icon = w.GetIconInfo(),
				};
			}).ToArray<IListItem>();
		}
		catch
		{
			return [];
		}
	}
}
