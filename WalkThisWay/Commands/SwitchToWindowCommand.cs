// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Commands;

internal sealed partial class SwitchToWindowCommand : InvokableCommand
{
	private readonly Window _window;

	public SwitchToWindowCommand(Window window)
	{
		_window = window;
		Name = Resources.switch_to_window;
		Icon = Icons.SwitchIcon;
	}

	public override ICommandResult Invoke()
	{
		ActivateWindow(_window.Hwnd);
		return CommandResult.Dismiss();
	}

	private static void ActivateWindow(IntPtr hwnd)
	{
		if (NativeMethods.IsIconic(hwnd))
		{
			NativeMethods.ShowWindow(hwnd, NativeMethods.SW_RESTORE);
		}

		NativeMethods.SetForegroundWindow(hwnd);
	}
}
