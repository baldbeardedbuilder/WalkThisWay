// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Helpers;

[GeneratedComInterface]
[Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
internal partial interface IVirtualDesktopManager
{
	[PreserveSig]
	int IsWindowOnCurrentVirtualDesktop(IntPtr hTopLevelWindow, out int onCurrentDesktop);

	[PreserveSig]
	int GetWindowDesktopId(IntPtr hTopLevelWindow, out Guid desktop);

	[PreserveSig]
	int MoveWindowToDesktop(IntPtr hTopLevelWindow, ref Guid desktop);
}
