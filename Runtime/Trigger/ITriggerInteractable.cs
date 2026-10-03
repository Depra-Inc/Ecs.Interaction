// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using Depra.Ecs.QoL;
using UnityEngine;

namespace Depra.Ecs.Interaction.Trigger
{
	public interface ITriggerInteractable
	{
		Transform Anchor { get; }

		TriggerInteractionMode Mode { get; }

		float Duration { get; }

		bool CanInteract(PackedEntityWithWorld with);

		void Begin(PackedEntityWithWorld with);
		void Progress(PackedEntityWithWorld with, float normalized);
		void Complete(PackedEntityWithWorld with);
		void Cancel(PackedEntityWithWorld with);
	}
}
