// SPDX-License-Identifier: Apache-2.0
// © 2024 Evgeniy Kraevskiy <e.craevsky@depra.org>

using Depra.Ecs.QoL;

namespace Depra.Ecs.Interaction
{
	public interface IInteractable
	{
		bool CanInteract { get; }
		void Begin(PackedEntityWithWorld sourceEntity);
		void Execute(PackedEntityWithWorld sourceEntity);
		void Finish(PackedEntityWithWorld sourceEntity);
	}
}