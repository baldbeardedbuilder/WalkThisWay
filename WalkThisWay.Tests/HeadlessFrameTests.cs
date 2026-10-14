// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.CmdPal.Ext.WalkThisWay.UnitTests;

[TestClass]
public class HeadlessFrameTests
{
	private const string FrameClass = "ApplicationFrameWindow";

	[TestMethod]
	public void CloakedFrameWithoutHostedWindow_IsHeadless()
	{
		Assert.IsTrue(Window.IsHeadlessAppFrame(FrameClass, true, () => false));
	}

	[TestMethod]
	public void CloakedFrameWithHostedWindow_IsNotHeadless()
	{
		Assert.IsFalse(Window.IsHeadlessAppFrame(FrameClass, true, () => true));
	}

	[TestMethod]
	public void UncloakedFrame_IsNotHeadless_AndSkipsProbe()
	{
		bool probed = false;
		bool result = Window.IsHeadlessAppFrame(FrameClass, false, () => probed = true);

		Assert.IsFalse(result);
		Assert.IsFalse(probed);
	}

	[TestMethod]
	public void CloakedNonFrameWindow_IsNotHeadless_AndSkipsProbe()
	{
		bool probed = false;
		bool result = Window.IsHeadlessAppFrame("Notepad", true, () => probed = true);

		Assert.IsFalse(result);
		Assert.IsFalse(probed);
	}
}
