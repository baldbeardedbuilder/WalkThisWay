// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Components;

internal sealed partial class WindowEventHook : IDisposable
{
	private static readonly Lazy<WindowEventHook> _instance = new(() => new WindowEventHook());

	internal static WindowEventHook Instance => _instance.Value;

	// Must be stored as a field to prevent the delegate from being garbage collected.
	private readonly NativeMethods.WinEventDelegate _winEventDelegate;
	private readonly Thread _messageThread;
	private readonly Lock _subscriberLock = new();
	private readonly List<Action> _subscribers = [];
	private readonly List<Action> _titleChangedSubscribers = [];
	private uint _messageThreadId;

	private WindowEventHook()
	{
		_winEventDelegate = OnWinEvent;

		_messageThread = new Thread(MessagePumpThread) { IsBackground = true, Name = "WinEventHookThread" };
		_messageThread.Start();
	}

	internal void Subscribe(Action callback)
	{
		lock (_subscriberLock)
		{
			_subscribers.Add(callback);
		}
	}

	internal void Unsubscribe(Action callback)
	{
		lock (_subscriberLock)
		{
			_subscribers.Remove(callback);
		}
	}

	internal void SubscribeTitleChanged(Action callback)
	{
		lock (_subscriberLock)
		{
			_titleChangedSubscribers.Add(callback);
		}
	}

	internal void UnsubscribeTitleChanged(Action callback)
	{
		lock (_subscriberLock)
		{
			_titleChangedSubscribers.Remove(callback);
		}
	}

	private void NotifyTitleChanged()
	{
		Action[] snapshot;
		lock (_subscriberLock)
		{
			snapshot = [.. _titleChangedSubscribers];
		}

		foreach (var callback in snapshot)
		{
			callback();
		}
	}

	private void NotifySubscribers()
	{
		Action[] snapshot;
		lock (_subscriberLock)
		{
			snapshot = [.. _subscribers];
		}

		foreach (var callback in snapshot)
		{
			callback();
		}
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
				NotifySubscribers();
			}
			else if (eventType == NativeMethods.EVENT_OBJECT_NAMECHANGE)
			{
				NotifyTitleChanged();
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
