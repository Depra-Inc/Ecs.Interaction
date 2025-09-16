// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

namespace Depra.Ecs.Interaction
{
	public sealed class PickupAspect : IComponentAspect
	{
		public ComponentPool<PickupRequest> Request { get; private set; }

		void IComponentAspect.Initialize(AspectGroup aspects, ComponentPoolGroup pools)
		{
			aspects.Add(this);
			pools.Add(Request = new ComponentPool<PickupRequest>());
		}
	}
}