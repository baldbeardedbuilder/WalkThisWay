// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.Storage.Streams;

namespace Microsoft.CmdPal.Ext.WalkThisWay;

internal static class Icons
{
	internal static IconInfo WalkThisWayIcon { get; } = LoadExtensionIcon();
	internal static IconInfo SwitchIcon { get; } = new("\uE8A7"); // Switch icon
	internal static IconInfo CloseIcon { get; } = new("\uE711"); // Close icon
	internal static IconInfo EndTaskIcon { get; } = new("\uE7E8"); // Cancel/stop icon
	internal static IconInfo GenericAppIcon { get; } = new("\uE737"); // Fallback window icon

	private static IconInfo LoadExtensionIcon()
	{
		try
		{
			var dir = AppContext.BaseDirectory;
			var logoPath = Path.Combine(dir, "Assets", "StoreLogo.png");

			if (File.Exists(logoPath))
			{
				return IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
			}
		}
		catch
		{
			// Fall through to default
		}

		return new IconInfo("\uE737");
	}

	/// <summary>
	/// Creates an IconInfo from PNG bytes using a WinRT stream reference.
	/// </summary>
	internal static IconInfo FromPngBytes(byte[] pngBytes)
	{
		var ms = new InMemoryRandomAccessStream();
		var writer = new DataWriter(ms.GetOutputStreamAt(0));
		writer.WriteBytes(pngBytes);
		writer.StoreAsync().AsTask().GetAwaiter().GetResult();
		writer.DetachStream();
		return IconInfo.FromStream(ms);
	}
}
