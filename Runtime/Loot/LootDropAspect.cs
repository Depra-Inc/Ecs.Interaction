// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

namespace Depra.Ecs.Interaction
{
	public sealed class LootDropAspect : IComponentAspect
	{
		public ComponentPool<LootPrefabsRef> Prefabs { get; private set; }
		
		void IComponentAspect.Initialize(AspectGroup aspects, ComponentPoolGroup pools)
		{
			aspects.Add(this);
			pools.Add(Prefabs = new ComponentPool<LootPrefabsRef>());
		}
	}
}