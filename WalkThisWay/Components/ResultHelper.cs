// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Ext.WalkThisWay.Commands;
using Microsoft.CmdPal.Ext.WalkThisWay.Pages;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.WalkThisWay.Components;

internal static class ResultHelper
{
	internal static IListItem[] GetResultList(IReadOnlyList<Window> windows, string query)
	{
		if (windows.Count == 0)
		{
			return [];
		}

		IEnumerable<Window> filtered = windows;

		if (!string.IsNullOrWhiteSpace(query))
		{
			var lowerQuery = query.ToLowerInvariant();
			filtered = windows
				.Select(w => new
				{
					Window = w,
					Score = ScoreWindow(w, lowerQuery),
				})
				.Where(x => x.Score > 0)
				.OrderByDescending(x => x.Score)
				.Select(x => x.Window);
		}

		return filtered.Select(CreateListItem).ToArray();
	}

	private static int ScoreWindow(Window window, string query)
	{
		int score = 0;

		var titleLower = window.Title.ToLowerInvariant();
		var processLower = window.ProcessName.ToLowerInvariant();

		// Exact substring match
		if (titleLower.Contains(query))
		{
			score += 100;
			if (titleLower.StartsWith(query, StringComparison.Ordinal))
			{
				score += 50;
			}
		}

		if (processLower.Contains(query))
		{
			score += 80;
			if (processLower.StartsWith(query, StringComparison.Ordinal))
			{
				score += 40;
			}
		}

		// Fuzzy character matching
		if (score == 0)
		{
			score += FuzzyMatch(titleLower, query) * 2;
			score += FuzzyMatch(processLower, query);
		}

		return score;
	}

	private static int FuzzyMatch(string text, string query)
	{
		int queryIndex = 0;
		int matches = 0;

		for (int i = 0; i < text.Length && queryIndex < query.Length; i++)
		{
			if (text[i] == query[queryIndex])
			{
				matches++;
				queryIndex++;
			}
		}

		// Only count as a match if all query characters were found in order
		return queryIndex == query.Length ? matches : 0;
	}

	private static WindowWalkerListItem CreateListItem(Window window)
	{
		var switchCommand = new SwitchToWindowCommand(window);

		var listItem = new WindowWalkerListItem(switchCommand)
		{
			Title = window.Title,
			Subtitle = window.ProcessName,
			Icon = window.GetIconInfo(),
			MoreCommands =
			[
				new CommandContextItem(new CloseWindowCommand(window))
				{
					Title = Resources.close_window,
					Icon = Icons.CloseIcon,
				},
				new CommandContextItem(new EndTaskCommand(window))
				{
					Title = Resources.end_task,
					Icon = Icons.EndTaskIcon,
				},
			],
		};

		return listItem;
	}
}
