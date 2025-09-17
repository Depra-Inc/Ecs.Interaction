// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Hybrid;
using Depra.Ecs.QoL;
using UnityEngine;
using static Depra.Ecs.Interaction.PickupModule;

namespace Depra.Ecs.Interaction
{
	[DisallowMultipleComponent]
	[AddComponentMenu(MENU_PATH + nameof(PickupParticle))]
	internal sealed class PickupParticle : MonoBehaviour
	{
		[SerializeField] private PickupItem _item;
		[SerializeField] private LayerMask _layerMask;

		private void OnParticleCollision(GameObject other)
		{
			if ((_layerMask.value & (1 << other.gameObject.layer)) == 0)
			{
				return;
			}
		
			if (other.TryGetComponent(out IAuthoringEntity authoringEntity) &&
			    authoringEntity.Unpack(out var world, out var entity))
			{
				world.Pool<PickupRequest>().GetOrAllocate(entity).Value = _item;
			}
		}
	}
}