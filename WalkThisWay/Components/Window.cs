// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Components;

internal sealed class Window
{
	private string? _processName;
	private string? _processFileName;
	private Microsoft.CommandPalette.Extensions.Toolkit.IconInfo? _iconInfo;
	private bool _iconResolved;

	public IntPtr Hwnd { get; }

	public string Title { get; }

	public uint ProcessId { get; }

	public string ProcessName => _processName ??= Win32Helpers.GetProcessName(ProcessId);

	public string ProcessFileName => _processFileName ??= Win32Helpers.GetProcessFileName(ProcessId);

	public string ClassName { get; }

	public bool IsVisible { get; }

	public bool IsOwner { get; }

	public bool IsToolWindow { get; }

	public bool IsAppWindow { get; }

	public bool IsTaskListDeleted { get; }

	public int CloakedState { get; }

	public bool IsCloaked => CloakedState != 0;

	public Window(IntPtr hwnd)
	{
		Hwnd = hwnd;
		Title = Win32Helpers.GetWindowTitle(hwnd);
		ClassName = Win32Helpers.GetClassName(hwnd);
		IsVisible = NativeMethods.IsWindowVisible(hwnd);
		IsOwner = Win32Helpers.IsOwnerWindow(hwnd);
		IsToolWindow = Win32Helpers.IsToolWindow(hwnd);
		IsAppWindow = Win32Helpers.IsAppWindow(hwnd);
		IsTaskListDeleted = Win32Helpers.IsTaskListDeleted(hwnd);
		CloakedState = Win32Helpers.GetCloakedState(hwnd);

		NativeMethods.GetWindowThreadProcessId(hwnd, out uint pid);
		ProcessId = pid;
	}

	public bool ShouldBeIncluded()
	{
		if (!NativeMethods.IsWindow(Hwnd))
		{
			return false;
		}

		if (!IsVisible)
		{
			return false;
		}

		if (!IsOwner)
		{
			return false;
		}

		if (IsToolWindow && !IsAppWindow)
		{
			return false;
		}

		if (IsTaskListDeleted)
		{
			return false;
		}

		// Skip cloaked windows (preloaded UWP apps, inherited cloak)
		if (CloakedState == NativeMethods.DWM_CLOAKED_APP ||
			CloakedState == NativeMethods.DWM_CLOAKED_INHERITED)
		{
			return false;
		}

		// Skip headless ApplicationFrameHost frames (cloaked, no real UI)
		if (IsHeadlessAppFrame(ClassName, IsCloaked, () => Win32Helpers.HasHostedAppWindow(Hwnd)))
		{
			return false;
		}

		// Skip Windows core window
		if (ClassName == "Windows.UI.Core.CoreWindow")
		{
			return false;
		}

		// Skip windows with empty titles
		if (string.IsNullOrWhiteSpace(Title))
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// A cloaked UWP frame with no hosted app window is an empty shell, not a real window.
	/// The hosted-window probe only runs once the cheaper checks have passed.
	/// </summary>
	internal static bool IsHeadlessAppFrame(string className, bool isCloaked, Func<bool> hasHostedAppWindow)
	{
		return isCloaked &&
			className == Win32Helpers.ApplicationFrameClassName &&
			!hasHostedAppWindow();
	}

	public IntPtr GetIcon()
	{
		return Win32Helpers.TryGetWindowIcon(Hwnd);
	}

	/// <summary>
	/// Gets the window's application icon as an IconInfo for the CmdPal SDK.
	/// Falls back to a generic app icon glyph if extraction fails.
	/// </summary>
	public Microsoft.CommandPalette.Extensions.Toolkit.IconInfo GetIconInfo()
	{
		if (!_iconResolved)
		{
			_iconInfo = Win32Helpers.TryGetWindowIconInfo(Hwnd);
			_iconResolved = true;
		}

		return _iconInfo ?? Icons.GenericAppIcon;
	}

	public override string ToString() => $"{Title} ({ProcessName})";
}
