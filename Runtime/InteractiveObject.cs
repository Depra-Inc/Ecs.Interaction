// SPDX-License-Identifier: Apache-2.0
// © 2024 Evgeniy Kraevskiy <e.craevsky@depra.org>

using Depra.Ecs.Hybrid;
using Depra.Ecs.QoL;
using UnityEngine;

namespace Depra.Ecs.Interaction
{
	public sealed class InteractiveObject : MonoBehaviour, IInteractable
	{
		[SerializeField] private ComponentDatabase[] _componentDatabases;
		
		public bool CanInteract { get; private set; } = true;

		public void Begin(PackedEntityWithWorld sourceEntity)
		{
			if (!sourceEntity.Unpack(out var world, out var interactorEntity))
			{
				return;
			}

			foreach (var componentDatabase in _componentDatabases)
			{
				foreach (var component in componentDatabase.Components)
				{
					var componentType = component.GetType();

					if (world.Pools.Contains(componentType))
					{
						world.Pools.Get(componentType).Allocate(interactorEntity, component);
					}
				}
			}

			CanInteract = false;
		}

		public void Execute(PackedEntityWithWorld sourceEntity)
		{		
			//TODO: Сделать таймер
			return;
		}

		public void Finish(PackedEntityWithWorld sourceEntity)
		{
			return;
		}
	}
}