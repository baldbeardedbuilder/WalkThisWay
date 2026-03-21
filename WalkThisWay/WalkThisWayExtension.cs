// Copyright (c) Bald Bearded Builder LLC
// Bald Bearded Builder LLC licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using Microsoft.CommandPalette.Extensions;

namespace WalkThisWay;

[Guid("d3a7b4c1-8e2f-4a5d-9c6b-1f3e5d7a9b2c")]
public sealed partial class WalkThisWayExtension : IExtension, IDisposable
{
	private readonly ManualResetEvent _extensionDisposedEvent;

	private readonly WalkThisWayCommandsProvider _provider = new();

	public WalkThisWayExtension(ManualResetEvent extensionDisposedEvent)
	{
		_extensionDisposedEvent = extensionDisposedEvent;
	}

	public object? GetProvider(ProviderType providerType)
	{
		return providerType switch
		{
			ProviderType.Commands => _provider,
			_ => null,
		};
	}

	public void Dispose()
	{
		_provider?.Dispose();
		_extensionDisposedEvent.Set();
	}
}
