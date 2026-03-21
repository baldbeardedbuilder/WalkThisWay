// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Helpers;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Components;

internal sealed class WindowProcess
{
	private static readonly Lock _cacheLock = new();
	private static readonly Dictionary<uint, WindowProcess> _cache = [];

	public uint ProcessId { get; }

	public string Name { get; }

	public string FileName { get; }

	public bool IsAccessible { get; }

	private WindowProcess(uint processId)
	{
		ProcessId = processId;
		Name = Win32Helpers.GetProcessName(processId);
		FileName = Win32Helpers.GetProcessFileName(processId);
		IsAccessible = TestProcessAccess(processId);
	}

	internal static WindowProcess Get(uint processId)
	{
		lock (_cacheLock)
		{
			if (_cache.Count > 5000)
			{
				_cache.Clear();
			}

			if (!_cache.TryGetValue(processId, out var process))
			{
				process = new WindowProcess(processId);
				_cache[processId] = process;
			}

			return process;
		}
	}

	internal static void ClearCache()
	{
		lock (_cacheLock)
		{
			_cache.Clear();
		}
	}

	private static bool TestProcessAccess(uint processId)
	{
		IntPtr hProcess = NativeMethods.OpenProcess(
			NativeMethods.PROCESS_TERMINATE, false, processId);

		if (hProcess == IntPtr.Zero)
		{
			return false;
		}

		NativeMethods.CloseHandle(hProcess);
		return true;
	}
}
