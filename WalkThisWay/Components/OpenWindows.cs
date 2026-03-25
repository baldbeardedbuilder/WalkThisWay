// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;
using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Components;

internal sealed class OpenWindows
{
	private static readonly Lock _updateLock = new();
	private static readonly Lazy<OpenWindows> _instance = new(() => new OpenWindows());

	internal static OpenWindows Instance => _instance.Value;

	private volatile ImmutableDictionary<IntPtr, Window> _windows = ImmutableDictionary<IntPtr, Window>.Empty;

	private OpenWindows()
	{
	}

	internal ImmutableDictionary<IntPtr, Window> GetWindows() => _windows;

	internal void UpdateOpenWindowsList()
	{
		lock (_updateLock)
		{
			var builder = ImmutableDictionary.CreateBuilder<IntPtr, Window>();

			NativeMethods.EnumWindows((hwnd, _) =>
			{
				var window = new Window(hwnd);
				if (window.ShouldBeIncluded())
				{
					builder[hwnd] = window;
				}

				return true;
			}, IntPtr.Zero);

			_windows = builder.ToImmutable();
		}
	}
}
