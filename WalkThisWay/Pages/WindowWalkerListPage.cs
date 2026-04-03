// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Pages;

internal sealed partial class WindowWalkerListPage : DynamicListPage, IDisposable
{
	// Must be stored as a field to prevent the delegate from being garbage collected.
	private readonly NativeMethods.WinEventDelegate _winEventDelegate;
	private readonly Thread _messageThread;
	private uint _messageThreadId;

	public WindowWalkerListPage()
	{
		Icon = Icons.WalkThisWayIcon;
		Name = Resources.plugin_name;
		Id = "com.baldbeardedbuilder.cmdpal.walkthisway";
		PlaceholderText = Resources.search_placeholder;

		_winEventDelegate = OnWinEvent;

		_messageThread = new Thread(MessagePumpThread) { IsBackground = true, Name = "WinEventHookThread" };
		_messageThread.Start();
	}

	private void MessagePumpThread()
	{
		_messageThreadId = NativeMethods.GetCurrentThreadId();

		uint flags = NativeMethods.WINEVENT_OUTOFCONTEXT | NativeMethods.WINEVENT_SKIPOWNPROCESS;

		var hooks = new[]
		{
			NativeMethods.SetWinEventHook(NativeMethods.EVENT_OBJECT_CREATE, NativeMethods.EVENT_OBJECT_CREATE, IntPtr.Zero, _winEventDelegate, 0, 0, flags),
			NativeMethods.SetWinEventHook(NativeMethods.EVENT_OBJECT_DESTROY, NativeMethods.EVENT_OBJECT_DESTROY, IntPtr.Zero, _winEventDelegate, 0, 0, flags),
			NativeMethods.SetWinEventHook(NativeMethods.EVENT_OBJECT_NAMECHANGE, NativeMethods.EVENT_OBJECT_NAMECHANGE, IntPtr.Zero, _winEventDelegate, 0, 0, flags),
			NativeMethods.SetWinEventHook(NativeMethods.EVENT_SYSTEM_FOREGROUND, NativeMethods.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, _winEventDelegate, 0, 0, flags),
			NativeMethods.SetWinEventHook(NativeMethods.EVENT_SYSTEM_MINIMIZESTART, NativeMethods.EVENT_SYSTEM_MINIMIZESTART, IntPtr.Zero, _winEventDelegate, 0, 0, flags),
			NativeMethods.SetWinEventHook(NativeMethods.EVENT_SYSTEM_MINIMIZEEND, NativeMethods.EVENT_SYSTEM_MINIMIZEEND, IntPtr.Zero, _winEventDelegate, 0, 0, flags),
		};

		try
		{
			while (NativeMethods.GetMessage(out var msg, IntPtr.Zero, 0, 0) > 0)
			{
				NativeMethods.TranslateMessage(in msg);
				NativeMethods.DispatchMessage(in msg);
			}
		}
		finally
		{
			foreach (var hook in hooks)
			{
				if (hook != IntPtr.Zero)
				{
					NativeMethods.UnhookWinEvent(hook);
				}
			}
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
			return ResultHelper.GetResultList(OpenWindows.Instance.GetWindows(), SearchText ?? string.Empty);
		}
		catch
		{
			return [];
		}
	}

	private void OnWinEvent(
		IntPtr hWinEventHook, uint eventType, IntPtr hwnd,
		int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
	{
		if (idObject != NativeMethods.OBJID_WINDOW || idChild != 0)
		{
			return;
		}

		try
		{
			var previousKeys = OpenWindows.Instance.GetWindows().Keys;
			OpenWindows.Instance.UpdateOpenWindowsList();
			var currentKeys = OpenWindows.Instance.GetWindows().Keys;

			if (!previousKeys.SequenceEqual(currentKeys))
			{
				RaiseItemsChanged(0);
			}
		}
		catch
		{
			// Swallow errors from event callback
		}
	}

	public void Dispose()
	{
		if (_messageThreadId != 0)
		{
			NativeMethods.PostThreadMessage(_messageThreadId, NativeMethods.WM_QUIT, IntPtr.Zero, IntPtr.Zero);
		}
	}
}
