// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Pooling;

namespace Depra.Ecs.Interaction
{
	public sealed class LootDropAspect : IComponentAspect
	{
		internal readonly PoolService Pooling;
		public LootDropAspect(PoolService pooling) => Pooling = pooling;

		public ComponentPool<LootDropPresetRef> Preset { get; private set; }
		
		void IComponentAspect.Initialize(AspectGroup aspects, ComponentPoolGroup pools)
		{
			aspects.Add(this);
			pools.Add(Preset = new ComponentPool<LootDropPresetRef>());
		}
	}
}