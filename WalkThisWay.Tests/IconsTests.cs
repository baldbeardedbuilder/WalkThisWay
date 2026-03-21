// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.CmdPal.Ext.WalkThisWay.UnitTests;

[TestClass]
public class IconsTests
{
	[TestMethod]
	public void WalkThisWayIcon_IsNotNull()
	{
		Assert.IsNotNull(Icons.WalkThisWayIcon);
	}

	[TestMethod]
	public void SwitchIcon_IsNotNull()
	{
		Assert.IsNotNull(Icons.SwitchIcon);
	}

	[TestMethod]
	public void CloseIcon_IsNotNull()
	{
		Assert.IsNotNull(Icons.CloseIcon);
	}

	[TestMethod]
	public void EndTaskIcon_IsNotNull()
	{
		Assert.IsNotNull(Icons.EndTaskIcon);
	}
}
