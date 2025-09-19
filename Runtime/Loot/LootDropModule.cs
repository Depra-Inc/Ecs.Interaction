// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Modules;
using Depra.Pooling;

namespace Depra.Ecs.Interaction
{
	public sealed class LootDropModule : IModule
	{
		private readonly PoolService _pooling;

		IComponentAspect[] IModule.Aspects => new IComponentAspect[]
		{
			new LootDropAspect(_pooling)
		};

		void IModule.Initialize(ISystemGroup systems) => systems.Add(new LootSpawnSystem());
	}
}