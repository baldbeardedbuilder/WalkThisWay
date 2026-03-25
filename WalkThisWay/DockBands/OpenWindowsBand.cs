// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Commands;
using Microsoft.CmdPal.Ext.WalkThisWay.Components;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.DockBands;

internal static class OpenWindowsBand
{
	private static readonly Lock _cacheLock = new();
	private static readonly Dictionary<IntPtr, CachedDockItem> _cache = [];

	internal static IListItem[] GetOpenWindowItems()
	{
		try
		{
			var windows = OpenWindows.Instance.GetWindows();

			lock (_cacheLock)
			{
				// Remove entries for windows that no longer exist
				var staleKeys = _cache.Keys.Where(k => !windows.ContainsKey(k)).ToList();
				foreach (var key in staleKeys)
				{
					_cache.Remove(key);
				}

				// Update or create entries for current windows
				foreach (var (hwnd, w) in windows)
				{
					if (_cache.TryGetValue(hwnd, out var cached))
					{
						if (cached.Title != w.Title || cached.ProcessName != w.ProcessName)
						{
							cached.Title = w.Title;
							cached.ProcessName = w.ProcessName;
							cached.Item.Title = w.Title;
							cached.Item.Subtitle = w.ProcessName;
							cached.Item.Icon = w.GetIconInfo();
						}
					}
					else
					{
						var switchCommand = new SwitchToWindowCommand(w);
						var item = new ListItem(switchCommand)
						{
							Title = w.Title,
							Subtitle = w.ProcessName,
							Icon = w.GetIconInfo(),
						};

						_cache[hwnd] = new CachedDockItem
						{
							Title = w.Title,
							ProcessName = w.ProcessName,
							Item = item,
						};
					}
				}

				return windows.Values.Select(w => _cache[w.Hwnd].Item).ToArray<IListItem>();
			}
		}
		catch
		{
			return [];
		}
	}

	private sealed class CachedDockItem
	{
		public required string Title { get; set; }

		public required string ProcessName { get; set; }

		public required ListItem Item { get; set; }
	}
}
