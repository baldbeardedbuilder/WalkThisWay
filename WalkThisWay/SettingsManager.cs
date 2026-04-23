// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay;

internal sealed class SettingsManager : JsonSettingsManager
{
	private const string Namespace = "walkThisWay";

	private static readonly Lazy<SettingsManager> _instance = new(() => new SettingsManager());

	internal static SettingsManager Instance => _instance.Value;

	private static string Namespaced(string propertyName) => $"{Namespace}.{propertyName}";

	private readonly ToggleSetting _dockCurrentDesktopOnly = new(
		Namespaced(nameof(DockCurrentDesktopOnly)),
		Resources.setting_dock_current_desktop_only,
		Resources.setting_dock_current_desktop_only_description,
		true);

	internal bool DockCurrentDesktopOnly => _dockCurrentDesktopOnly.Value;

	private SettingsManager()
	{
		FilePath = SettingsJsonPath();

		Settings.Add(_dockCurrentDesktopOnly);

		LoadSettings();

		Settings.SettingsChanged += (_, _) => SaveSettings();
	}

	private static string SettingsJsonPath()
	{
		var directory = Utilities.BaseSettingsPath("WalkThisWay");
		Directory.CreateDirectory(directory);
		return Path.Combine(directory, "settings.json");
	}
}
