// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Hybrid;
using Depra.Ecs.QoL;
using UnityEngine;
using static Depra.Ecs.Interaction.PickUpCollectionModule;

namespace Depra.Ecs.Interaction
{
	[DisallowMultipleComponent]
	[AddComponentMenu(MENU_PATH + nameof(CollectionTrigger))]
	public sealed class CollectionTrigger : MonoBehaviour
	{
		[SerializeField] private AuthoringEntity _authoringEntity;

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent<IInteractable>(out var interactable))
			{
				return;
			}

			if (_authoringEntity.Unpack(out var world, out var entity))
			{
				world.Pool<PickUpRequest>().GetOrAllocate(entity).Value = interactable;
			}
		}
	}
}