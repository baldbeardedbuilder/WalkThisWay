// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Commands;

internal sealed partial class EndTaskCommand : InvokableCommand
{
	private readonly Window _window;

	public EndTaskCommand(Window window)
	{
		_window = window;
		Name = Resources.end_task;
		Icon = Icons.EndTaskIcon;
	}

	public override ICommandResult Invoke()
	{
		try
		{
			IntPtr hProcess = NativeMethods.OpenProcess(
				NativeMethods.PROCESS_TERMINATE,
				false,
				_window.ProcessId);

			if (hProcess != IntPtr.Zero)
			{
				try
				{
					NativeMethods.TerminateProcess(hProcess, 1);
				}
				finally
				{
					NativeMethods.CloseHandle(hProcess);
				}
			}
		}
		catch
		{
			// If we can't terminate the process, just dismiss
		}

		return CommandResult.Dismiss();
	}
}
