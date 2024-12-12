// SPDX-License-Identifier: Apache-2.0
// © 2024 Evgeniy Kraevskiy <e.craevsky@depra.org>

namespace Depra.Ecs.Interaction
{
	public sealed class InteractionAspect : IComponentAspect
	{
		public ComponentPool<InteractionLayerMask> Mask { get; private set; }
		public ComponentPool<InteractInputButton> Button { get; private set; }
		public ComponentPool<InteractionDistance> Distance { get; private set; }


		void IComponentAspect.Initialize(AspectGroup aspects, ComponentPoolGroup pools)
		{
			aspects.Add(this);
			pools.Add(Mask = new ComponentPool<InteractionLayerMask>());
			pools.Add(Button = new ComponentPool<InteractInputButton>());
			pools.Add(Distance = new ComponentPool<InteractionDistance>());
		}
	}
}