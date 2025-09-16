// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Hybrid;
using Depra.Ecs.QoL;
using UnityEngine;
using static Depra.Ecs.Interaction.PickupModule;

namespace Depra.Ecs.Interaction
{
	[DisallowMultipleComponent]
	[AddComponentMenu(MENU_PATH + nameof(PickupTrigger))]
	internal sealed class PickupTrigger : MonoBehaviour
	{
		[SerializeField] private AuthoringEntity _authoringEntity;

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent<PickupItem>(out var item))
			{
				return;
			}

			if (_authoringEntity.Unpack(out var world, out var entity))
			{
				world.Pool<PickupRequest>().GetOrAllocate(entity).Value = item;
			}
		}
	}
}