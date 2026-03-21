// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.CmdPal.Ext.WalkThisWay.UnitTests;

[TestClass]
public class ResourcesTests
{
	[TestMethod]
	public void PluginName_IsNotNullOrEmpty()
	{
		Assert.IsFalse(string.IsNullOrEmpty(Resources.plugin_name));
	}

	[TestMethod]
	public void PluginDescription_IsNotNullOrEmpty()
	{
		Assert.IsFalse(string.IsNullOrEmpty(Resources.plugin_description));
	}

	[TestMethod]
	public void SwitchToWindow_IsNotNullOrEmpty()
	{
		Assert.IsFalse(string.IsNullOrEmpty(Resources.switch_to_window));
	}

	[TestMethod]
	public void CloseWindow_IsNotNullOrEmpty()
	{
		Assert.IsFalse(string.IsNullOrEmpty(Resources.close_window));
	}

	[TestMethod]
	public void EndTask_IsNotNullOrEmpty()
	{
		Assert.IsFalse(string.IsNullOrEmpty(Resources.end_task));
	}

	[TestMethod]
	public void SearchPlaceholder_IsNotNullOrEmpty()
	{
		Assert.IsFalse(string.IsNullOrEmpty(Resources.search_placeholder));
	}

	[TestMethod]
	public void DockBandTitle_IsNotNullOrEmpty()
	{
		Assert.IsFalse(string.IsNullOrEmpty(Resources.dock_band_title));
	}
}
