// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Modules;

namespace Depra.Ecs.Interaction
{
	public sealed class LootDropModule : IModule
	{
		IComponentAspect[] IModule.Aspects => new IComponentAspect[]
		{
			new LootDropAspect()
		};

		void IModule.Initialize(ISystemGroup systems) => systems.Add(new LootSpawnSystem());
	}
}