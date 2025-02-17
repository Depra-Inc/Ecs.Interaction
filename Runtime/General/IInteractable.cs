// SPDX-License-Identifier: Apache-2.0
// © 2024 Depra <e.craevsky@depra.org>
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.QoL;

namespace Depra.Ecs.Interaction
{
	public interface IInteractable
	{
		bool CanInteract(PackedEntityWithWorld with);

		void Begin(PackedEntityWithWorld with);
		void Execute(PackedEntityWithWorld with);
		void Finish(PackedEntityWithWorld with);
	}
}