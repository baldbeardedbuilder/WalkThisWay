// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Helpers;

internal static class Win32Helpers
{
	internal static string GetWindowTitle(IntPtr hwnd)
	{
		int length = NativeMethods.GetWindowTextLength(hwnd);
		if (length == 0)
		{
			return string.Empty;
		}

		char[] buffer = new char[length + 1];
		NativeMethods.GetWindowText(hwnd, buffer, buffer.Length);
		return new string(buffer, 0, length);
	}

	internal static string GetClassName(IntPtr hwnd)
	{
		char[] buffer = new char[256];
		int length = NativeMethods.GetClassName(hwnd, buffer, buffer.Length);
		return length > 0 ? new string(buffer, 0, length) : string.Empty;
	}

	internal static string GetProcessName(uint processId)
	{
		try
		{
			using var process = System.Diagnostics.Process.GetProcessById((int)processId);
			return process.ProcessName;
		}
		catch
		{
			return string.Empty;
		}
	}

	internal static string GetProcessFileName(uint processId)
	{
		IntPtr hProcess = NativeMethods.OpenProcess(
			NativeMethods.PROCESS_QUERY_LIMITED_INFORMATION, false, processId);

		if (hProcess == IntPtr.Zero)
		{
			return string.Empty;
		}

		try
		{
			char[] buffer = new char[1024];
			uint size = (uint)buffer.Length;
			if (NativeMethods.QueryFullProcessImageName(hProcess, 0, buffer, ref size))
			{
				return new string(buffer, 0, (int)size);
			}

			return string.Empty;
		}
		finally
		{
			NativeMethods.CloseHandle(hProcess);
		}
	}

	internal static bool IsToolWindow(IntPtr hwnd)
	{
		IntPtr exStyle = NativeMethods.GetWindowLongPtr(hwnd, NativeMethods.GWL_EXSTYLE);
		return ((long)exStyle & NativeMethods.WS_EX_TOOLWINDOW) != 0;
	}

	internal static bool IsAppWindow(IntPtr hwnd)
	{
		IntPtr exStyle = NativeMethods.GetWindowLongPtr(hwnd, NativeMethods.GWL_EXSTYLE);
		return ((long)exStyle & NativeMethods.WS_EX_APPWINDOW) != 0;
	}

	internal static bool IsOwnerWindow(IntPtr hwnd)
	{
		return NativeMethods.GetWindow(hwnd, NativeMethods.GW_OWNER) == IntPtr.Zero;
	}

	internal static bool IsTaskListDeleted(IntPtr hwnd)
	{
		return NativeMethods.GetProp(hwnd, "ITaskList_Deleted") != IntPtr.Zero;
	}

	internal static int GetCloakedState(IntPtr hwnd)
	{
		int cloaked = 0;
		NativeMethods.DwmGetWindowAttribute(hwnd, NativeMethods.DWMWA_CLOAKED, out cloaked, sizeof(int));
		return cloaked;
	}

	internal static IntPtr TryGetWindowIcon(IntPtr hwnd, int timeoutMs = 100)
	{
		uint timeout = (uint)timeoutMs;

		// Try WM_GETICON with ICON_BIG
		if (NativeMethods.SendMessageTimeout(hwnd, NativeMethods.WM_GETICON, NativeMethods.ICON_BIG, IntPtr.Zero,
			NativeMethods.SMTO_ABORTIFHUNG, timeout, out IntPtr iconHandle) != IntPtr.Zero && iconHandle != IntPtr.Zero)
		{
			return iconHandle;
		}

		// Try WM_GETICON with ICON_SMALL
		if (NativeMethods.SendMessageTimeout(hwnd, NativeMethods.WM_GETICON, NativeMethods.ICON_SMALL, IntPtr.Zero,
			NativeMethods.SMTO_ABORTIFHUNG, timeout, out iconHandle) != IntPtr.Zero && iconHandle != IntPtr.Zero)
		{
			return iconHandle;
		}

		// Try WM_GETICON with ICON_SMALL2
		if (NativeMethods.SendMessageTimeout(hwnd, NativeMethods.WM_GETICON, NativeMethods.ICON_SMALL2, IntPtr.Zero,
			NativeMethods.SMTO_ABORTIFHUNG, timeout, out iconHandle) != IntPtr.Zero && iconHandle != IntPtr.Zero)
		{
			return iconHandle;
		}

		// Fallback to class icon
		iconHandle = NativeMethods.GetClassLongPtr(hwnd, NativeMethods.GCLP_HICON);
		if (iconHandle != IntPtr.Zero)
		{
			return iconHandle;
		}

		return NativeMethods.GetClassLongPtr(hwnd, NativeMethods.GCLP_HICONSM);
	}

	/// <summary>
	/// Converts a window's HICON to an IconInfo using WinRT streams.
	/// Returns null if the icon cannot be extracted.
	/// </summary>
	internal static IconInfo? TryGetWindowIconInfo(IntPtr hwnd)
	{
		try
		{
			var hIcon = TryGetWindowIcon(hwnd);
			if (hIcon == IntPtr.Zero)
			{
				return null;
			}

			using var icon = Icon.FromHandle(hIcon);
			using var bitmap = icon.ToBitmap();
			using var ms = new MemoryStream();
			bitmap.Save(ms, ImageFormat.Png);
			var pngBytes = ms.ToArray();
			return Icons.FromPngBytes(pngBytes);
		}
		catch
		{
			return null;
		}
	}
}
