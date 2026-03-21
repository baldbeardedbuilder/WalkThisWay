// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.CmdPal.Ext.WalkThisWay.UnitTests;

[TestClass]
public class WindowProcessTests
{
	[TestMethod]
	public void ClearCache_DoesNotThrow()
	{
		WindowProcess.ClearCache();
	}

	[TestMethod]
	public void Get_CurrentProcess_ReturnsValidProcess()
	{
		uint currentPid = (uint)Environment.ProcessId;
		var process = WindowProcess.Get(currentPid);

		Assert.IsNotNull(process);
		Assert.AreEqual(currentPid, process.ProcessId);
		Assert.IsFalse(string.IsNullOrEmpty(process.Name));
	}

	[TestMethod]
	public void Get_SameProcessTwice_ReturnsCachedInstance()
	{
		WindowProcess.ClearCache();
		uint currentPid = (uint)Environment.ProcessId;

		var process1 = WindowProcess.Get(currentPid);
		var process2 = WindowProcess.Get(currentPid);

		Assert.AreSame(process1, process2);
	}

	[TestMethod]
	public void Get_InvalidProcessId_ReturnsProcessWithEmptyName()
	{
		WindowProcess.ClearCache();
		// Use a very high PID that's unlikely to exist
		var process = WindowProcess.Get(999999);

		Assert.IsNotNull(process);
		Assert.AreEqual(999999u, process.ProcessId);
	}
}
