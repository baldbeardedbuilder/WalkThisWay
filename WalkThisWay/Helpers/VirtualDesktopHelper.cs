// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Helpers;

internal static class VirtualDesktopHelper
{
	private static readonly Lazy<IVirtualDesktopManager?> _manager = new(CreateManager);

	private static IVirtualDesktopManager? CreateManager()
	{
		try
		{
			var cw = new StrategyBasedComWrappers();
			var clsid = new Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a");
			var iid = new Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b");

			int hr = Ole32.CoCreateInstance(in clsid, IntPtr.Zero, 0x1 /* CLSCTX_INPROC_SERVER */, in iid, out var ppv);
			if (hr < 0 || ppv == IntPtr.Zero)
			{
				return null;
			}

			return (IVirtualDesktopManager)cw.GetOrCreateObjectForComInstance(ppv, CreateObjectFlags.None);
		}
		catch
		{
			return null;
		}
	}

	internal static bool IsWindowOnCurrentDesktop(IntPtr hwnd)
	{
		var manager = _manager.Value;
		if (manager == null)
		{
			return true;
		}

		try
		{
			var hr = manager.IsWindowOnCurrentVirtualDesktop(hwnd, out var onCurrentDesktop);
			return hr != 0 || onCurrentDesktop != 0;
		}
		catch
		{
			return true;
		}
	}

	internal static Guid GetWindowDesktopId(IntPtr hwnd)
	{
		var manager = _manager.Value;
		if (manager == null)
		{
			return Guid.Empty;
		}

		try
		{
			var hr = manager.GetWindowDesktopId(hwnd, out var desktop);
			return hr == 0 ? desktop : Guid.Empty;
		}
		catch
		{
			return Guid.Empty;
		}
	}

	private static class Ole32
	{
		[DllImport("ole32.dll")]
		internal static extern int CoCreateInstance(
			in Guid rclsid,
			IntPtr pUnkOuter,
			uint dwClsContext,
			in Guid riid,
			out IntPtr ppv);
	}
}
