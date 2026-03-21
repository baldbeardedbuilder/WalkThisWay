// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Shmuelie.WinRTServer;
using Shmuelie.WinRTServer.CsWinRT;

namespace WalkThisWay;

public class Program
{
	[MTAThread]
	public static void Main(string[] args)
	{
		if (args.Length > 0 && args[0] == "-RegisterProcessAsComServer")
		{
			global::Shmuelie.WinRTServer.ComServer server = new();

			ManualResetEvent extensionDisposedEvent = new(false);

			WalkThisWayExtension extensionInstance = new(extensionDisposedEvent);
			server.RegisterClass<WalkThisWayExtension, IExtension>(() => extensionInstance);
			server.Start();

			extensionDisposedEvent.WaitOne();
			server.Stop();
			server.UnsafeDispose();
		}
		else
		{
			Console.WriteLine("Not being launched as an Extension... exiting.");
		}
	}
}
