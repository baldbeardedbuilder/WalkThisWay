// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Helpers;

internal static partial class NativeMethods
{
	// Delegates
	internal delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

	// Window enumeration
	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool IsWindowVisible(IntPtr hWnd);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool IsWindow(IntPtr hWnd);

	// Window text
	[LibraryImport("user32.dll", EntryPoint = "GetWindowTextW", StringMarshalling = StringMarshalling.Utf16)]
	internal static partial int GetWindowText(IntPtr hWnd, [Out] char[] lpString, int nMaxCount);

	[LibraryImport("user32.dll", EntryPoint = "GetWindowTextLengthW")]
	internal static partial int GetWindowTextLength(IntPtr hWnd);

	// Window styles
	[LibraryImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
	internal static partial IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

	internal const int GWL_EXSTYLE = -20;
	internal const int WS_EX_TOOLWINDOW = 0x00000080;
	internal const int WS_EX_APPWINDOW = 0x00040000;

	// Window owner
	[LibraryImport("user32.dll")]
	internal static partial IntPtr GetWindow(IntPtr hWnd, uint uCmd);

	internal const uint GW_OWNER = 4;

	// Process ID
	[LibraryImport("user32.dll")]
	internal static partial uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

	// Window class name
	[LibraryImport("user32.dll", EntryPoint = "GetClassNameW", StringMarshalling = StringMarshalling.Utf16)]
	internal static partial int GetClassName(IntPtr hWnd, [Out] char[] lpClassName, int nMaxCount);

	// Window properties (for ITaskList_Deleted)
	[LibraryImport("user32.dll", EntryPoint = "GetPropW", StringMarshalling = StringMarshalling.Utf16)]
	internal static partial IntPtr GetProp(IntPtr hWnd, string lpString);

	// DWM cloaking
	[LibraryImport("dwmapi.dll")]
	internal static partial int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out int pvAttribute, int cbAttribute);

	internal const int DWMWA_CLOAKED = 14;
	internal const int DWM_CLOAKED_APP = 1;
	internal const int DWM_CLOAKED_SHELL = 2;
	internal const int DWM_CLOAKED_INHERITED = 4;

	// Focus / activate window
	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool SetForegroundWindow(IntPtr hWnd);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

	internal const int SW_RESTORE = 9;
	internal const int SW_SHOW = 5;
	internal const int SW_MINIMIZE = 6;

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool IsIconic(IntPtr hWnd);

	// SendMessage
	[LibraryImport("user32.dll", EntryPoint = "SendMessageW")]
	internal static partial IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

	[LibraryImport("user32.dll", EntryPoint = "SendMessageTimeoutW")]
	internal static partial IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

	internal const uint WM_CLOSE = 0x0010;
	internal const uint WM_GETICON = 0x007F;
	internal const uint WM_SYSCOMMAND = 0x0112;
	internal const uint SC_RESTORE = 0xF120;
	internal const uint SMTO_ABORTIFHUNG = 0x0002;

	internal static readonly IntPtr ICON_SMALL = IntPtr.Zero;
	internal static readonly IntPtr ICON_BIG = new(1);
	internal static readonly IntPtr ICON_SMALL2 = new(2);

	// Icon extraction
	[LibraryImport("user32.dll", EntryPoint = "GetClassLongPtrW")]
	internal static partial IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex);

	internal const int GCLP_HICON = -14;
	internal const int GCLP_HICONSM = -34;

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool DestroyIcon(IntPtr hIcon);

	// Process access
	[LibraryImport("kernel32.dll")]
	internal static partial IntPtr OpenProcess(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);

	[LibraryImport("kernel32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool CloseHandle(IntPtr hObject);

	[LibraryImport("kernel32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool TerminateProcess(IntPtr hProcess, uint uExitCode);

	internal const uint PROCESS_TERMINATE = 0x0001;
	internal const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;

	// Process name
	[LibraryImport("kernel32.dll", EntryPoint = "QueryFullProcessImageNameW", StringMarshalling = StringMarshalling.Utf16)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool QueryFullProcessImageName(IntPtr hProcess, uint dwFlags, [Out] char[] lpExeName, ref uint lpdwSize);

	// WinEvent hooks
	internal delegate void WinEventDelegate(
		IntPtr hWinEventHook, uint eventType, IntPtr hwnd,
		int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

	[LibraryImport("user32.dll")]
	internal static partial IntPtr SetWinEventHook(
		uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
		WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool UnhookWinEvent(IntPtr hWinEventHook);

	internal const uint EVENT_OBJECT_CREATE = 0x8000;
	internal const uint EVENT_OBJECT_DESTROY = 0x8001;
	internal const uint EVENT_OBJECT_NAMECHANGE = 0x800C;
	internal const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
	internal const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;
	internal const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;
	internal const uint WINEVENT_OUTOFCONTEXT = 0x0000;
	internal const uint WINEVENT_SKIPOWNPROCESS = 0x0002;
	internal const int OBJID_WINDOW = 0;

	// Message pump
	[LibraryImport("user32.dll", EntryPoint = "GetMessageW")]
	internal static partial int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

	[LibraryImport("user32.dll", EntryPoint = "TranslateMessage")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool TranslateMessage(in MSG lpMsg);

	[LibraryImport("user32.dll", EntryPoint = "DispatchMessageW")]
	internal static partial IntPtr DispatchMessage(in MSG lpMsg);

	[LibraryImport("user32.dll", EntryPoint = "PostThreadMessageW")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool PostThreadMessage(uint idThread, uint Msg, IntPtr wParam, IntPtr lParam);

	[LibraryImport("kernel32.dll")]
	internal static partial uint GetCurrentThreadId();

	internal const uint WM_QUIT = 0x0012;

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
	internal struct MSG
	{
		internal IntPtr hwnd;
		internal uint message;
		internal IntPtr wParam;
		internal IntPtr lParam;
		internal uint time;
		internal int pt_x;
		internal int pt_y;
	}
}
