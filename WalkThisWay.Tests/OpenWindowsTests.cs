// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.CmdPal.Ext.WalkThisWay.UnitTests;

[TestClass]
public class OpenWindowsTests
{
	[TestMethod]
	public void Instance_ReturnsSingleton()
	{
		var instance1 = OpenWindows.Instance;
		var instance2 = OpenWindows.Instance;

		Assert.AreSame(instance1, instance2);
	}

	[TestMethod]
	public void GetWindows_ReturnsNonNullDictionary()
	{
		var windows = OpenWindows.Instance.GetWindows();

		Assert.IsNotNull(windows);
	}

	[TestMethod]
	public void GetWindows_ReturnsOnlyVisibleWindows()
	{
		OpenWindows.Instance.UpdateOpenWindowsList();
		var windows = OpenWindows.Instance.GetWindows();

		foreach (var window in windows.Values)
		{
			Assert.IsTrue(window.IsVisible, $"Window '{window.Title}' should be visible");
			Assert.IsFalse(string.IsNullOrWhiteSpace(window.Title), "Window should have a title");
		}
	}

	[TestMethod]
	public void UpdateOpenWindowsList_DoesNotThrow()
	{
		OpenWindows.Instance.UpdateOpenWindowsList();
	}
}
