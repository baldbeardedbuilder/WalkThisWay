// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Components;

internal sealed class OpenWindows
{
	private static readonly Lock _enumLock = new();
	private static readonly Lazy<OpenWindows> _instance = new(() => new OpenWindows());

	internal static OpenWindows Instance => _instance.Value;

	private readonly List<Window> _windows = [];

	private OpenWindows()
	{
	}

	internal IReadOnlyList<Window> GetWindows()
	{
		lock (_enumLock)
		{
			return [.. _windows];
		}
	}

	internal void UpdateOpenWindowsList()
	{
		lock (_enumLock)
		{
			_windows.Clear();
		}

		NativeMethods.EnumWindows(WindowEnumerationCallback, IntPtr.Zero);
	}

	private bool WindowEnumerationCallback(IntPtr hwnd, IntPtr lParam)
	{
		var window = new Window(hwnd);

		if (window.ShouldBeIncluded())
		{
			lock (_enumLock)
			{
				_windows.Add(window);
			}
		}

		return true;
	}
}
