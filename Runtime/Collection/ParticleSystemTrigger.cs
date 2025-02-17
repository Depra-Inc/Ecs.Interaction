// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Hybrid;
using Depra.Ecs.QoL;
using UnityEngine;
using static Depra.Ecs.Interaction.PickUpCollectionModule;

namespace Depra.Ecs.Interaction
{
	[DisallowMultipleComponent]
	[AddComponentMenu(MENU_PATH + nameof(ParticleSystemTrigger))]
	internal sealed class ParticleSystemTrigger : MonoBehaviour
	{
		[SerializeField] private LayerMask _layerMask;
		[SerializeField] private InteractionObject _interactionObject;

		private IInteractable Interactable => _interactionObject;

		private void OnParticleCollision(GameObject other)
		{
			if ((_layerMask.value & (1 << other.gameObject.layer)) == 0 ||
			    !other.TryGetComponent(out IAuthoringEntity authoringEntity))
			{
				return;
			}

			if (authoringEntity.Unpack(out var world, out var entity))
			{
				world.Pool<PickUpRequest>().GetOrAllocate(entity).Value = Interactable;
			}
		}
	}
}