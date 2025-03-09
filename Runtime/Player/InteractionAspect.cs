// SPDX-License-Identifier: Apache-2.0
// © 2024 Depra <e.craevsky@depra.org>
// © 2025 Depra <n.melnikov@depra.org>

namespace Depra.Ecs.Interaction
{
	public sealed class InteractionAspect : IComponentAspect
	{
		public ComponentPool<InteractionLayerMask> Mask { get; private set; }
		public ComponentPool<InteractInputActionRef> Button { get; private set; }
		public ComponentPool<InteractionDistance> Distance { get; private set; }

		void IComponentAspect.Initialize(AspectGroup aspects, ComponentPoolGroup pools)
		{
			aspects.Add(this);
			pools.Add(Mask = new ComponentPool<InteractionLayerMask>());
			pools.Add(Button = new ComponentPool<InteractInputActionRef>());
			pools.Add(Distance = new ComponentPool<InteractionDistance>());
		}
	}
}