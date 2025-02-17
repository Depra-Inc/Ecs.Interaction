// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

namespace Depra.Ecs.Interaction
{
	public sealed class PickUpCollectionAspect : IComponentAspect
	{
		public ComponentPool<CollectorTag> Collector { get; private set; }
		public ComponentPool<PickUpRequest> UpRequest { get; private set; }

		void IComponentAspect.Initialize(AspectGroup aspects, ComponentPoolGroup pools)
		{
			aspects.Add(this);
			pools.Add(Collector = new ComponentPool<CollectorTag>());
			pools.Add(UpRequest = new ComponentPool<PickUpRequest>());
		}
	}
}