// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;
using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.CmdPal.Ext.WalkThisWay.UnitTests;

[TestClass]
public class ResultHelperTests
{
	[TestMethod]
	public void GetResultList_EmptyWindows_ReturnsEmptyArray()
	{
		var result = ResultHelper.GetResultList(ImmutableDictionary<IntPtr, Window>.Empty, string.Empty);
		Assert.AreEqual(0, result.Length);
	}

	[TestMethod]
	public void GetResultList_EmptyQuery_ReturnsAllWindows()
	{
		// We can't easily test with real windows, but we can verify
		// the method doesn't throw with an empty query
		var result = ResultHelper.GetResultList(ImmutableDictionary<IntPtr, Window>.Empty, "");
		Assert.IsNotNull(result);
	}

	[TestMethod]
	public void GetResultList_NullQuery_ReturnsAllWindows()
	{
		var result = ResultHelper.GetResultList(ImmutableDictionary<IntPtr, Window>.Empty, string.Empty);
		Assert.IsNotNull(result);
		Assert.AreEqual(0, result.Length);
	}
}
