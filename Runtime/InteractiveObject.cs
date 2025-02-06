// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Evgeniy Kraevskiy <e.craevsky@depra.org>

using Depra.Ecs.Hybrid;
using Depra.Ecs.QoL;
using UnityEngine;
using UnityEngine.Events;

namespace Depra.Ecs.Interaction
{
	public sealed class InteractiveObject : MonoBehaviour, IInteractable
	{
		[SerializeField] private ComponentDatabase[] _onBeginComponentDatabases;
		[SerializeField] private ComponentDatabase[] _onExecuteComponentDatabases;
		[SerializeField] private ComponentDatabase[] _onFinishComponentDatabases;

		[SerializeField] private UnityEvent _onBegin;
		[SerializeField] private UnityEvent _onFinish;
		public bool CanInteract { get; private set; } = true;

		public void Begin(PackedEntityWithWorld sourceEntity)
		{
			if (!sourceEntity.Unpack(out var world, out var interactorEntity))
			{
				return;
			}

			foreach (var componentDatabase in _onBeginComponentDatabases)
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
			_onBegin.Invoke();
			CanInteract = false;
		}

		public void Execute(PackedEntityWithWorld sourceEntity)
		{		
			//TODO: Сделать таймер
			return;
		}

		public void Finish(PackedEntityWithWorld sourceEntity)
		{
			_onFinish.Invoke();
			return;
		}
	}
}