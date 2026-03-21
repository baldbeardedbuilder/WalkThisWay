// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Commands;

internal sealed partial class CloseWindowCommand : InvokableCommand
{
	private readonly Window _window;

	public CloseWindowCommand(Window window)
	{
		_window = window;
		Name = Resources.close_window;
		Icon = Icons.CloseIcon;
	}

	public override ICommandResult Invoke()
	{
		NativeMethods.SendMessage(
			_window.Hwnd,
			NativeMethods.WM_CLOSE,
			IntPtr.Zero,
			IntPtr.Zero);

		return CommandResult.Dismiss();
	}
}
