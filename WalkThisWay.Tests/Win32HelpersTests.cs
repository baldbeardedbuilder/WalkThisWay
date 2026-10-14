// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.CmdPal.Ext.WalkThisWay.UnitTests;

[TestClass]
public class Win32HelpersTests
{
	[TestMethod]
	public void GetWindowTitle_InvalidHandle_ReturnsEmptyString()
	{
		string title = Win32Helpers.GetWindowTitle(IntPtr.Zero);
		Assert.AreEqual(string.Empty, title);
	}

	[TestMethod]
	public void GetClassName_InvalidHandle_ReturnsEmptyString()
	{
		string className = Win32Helpers.GetClassName(IntPtr.Zero);
		Assert.AreEqual(string.Empty, className);
	}

	[TestMethod]
	public void GetProcessName_CurrentProcess_ReturnsNonEmpty()
	{
		uint pid = (uint)Environment.ProcessId;
		string name = Win32Helpers.GetProcessName(pid);
		Assert.IsFalse(string.IsNullOrEmpty(name));
	}

	[TestMethod]
	public void GetProcessName_InvalidPid_ReturnsEmpty()
	{
		string name = Win32Helpers.GetProcessName(999999);
		Assert.AreEqual(string.Empty, name);
	}

	[TestMethod]
	public void IsToolWindow_InvalidHandle_ReturnsFalse()
	{
		bool result = Win32Helpers.IsToolWindow(IntPtr.Zero);
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void IsAppWindow_InvalidHandle_ReturnsFalse()
	{
		bool result = Win32Helpers.IsAppWindow(IntPtr.Zero);
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void IsOwnerWindow_InvalidHandle_ReturnsTrue()
	{
		// Zero handle has no owner, so IsOwner returns true
		bool result = Win32Helpers.IsOwnerWindow(IntPtr.Zero);
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void IsTaskListDeleted_InvalidHandle_ReturnsFalse()
	{
		bool result = Win32Helpers.IsTaskListDeleted(IntPtr.Zero);
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void GetCloakedState_InvalidHandle_ReturnsZero()
	{
		int state = Win32Helpers.GetCloakedState(IntPtr.Zero);
		Assert.AreEqual(0, state);
	}

	[TestMethod]
	public void HasHostedAppWindow_InvalidHandle_ReturnsFalse()
	{
		Assert.IsFalse(Win32Helpers.HasHostedAppWindow(IntPtr.Zero));
	}

	[TestMethod]
	public void TryGetWindowIcon_InvalidHandle_ReturnsZero()
	{
		IntPtr icon = Win32Helpers.TryGetWindowIcon(IntPtr.Zero);
		Assert.AreEqual(IntPtr.Zero, icon);
	}
}
