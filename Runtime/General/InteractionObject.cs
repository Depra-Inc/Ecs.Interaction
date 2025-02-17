// SPDX-License-Identifier: Apache-2.0
// © 2024 Depra <e.craevsky@depra.org>
// © 2025 Depra <n.melnikov@depra.org>

using System;
using System.Runtime.CompilerServices;
using Depra.Ecs.Hybrid;
using Depra.Ecs.QoL;
using UnityEngine;

namespace Depra.Ecs.Interaction
{
	[AddComponentMenu("Ecs/Interaction/Interaction Object")]
	public sealed class InteractionObject : MonoBehaviour, IInteractable
	{
		[SerializeField] private ComponentDatabase[] _componentsToAdd;
		[Min(-1)] [SerializeField] private int _useLimit = -1;

		private int _useCount;

		public event Action InteractionStarted;
		public event Action InteractionFinished;

		private bool CanInteract => _useLimit == -1 || _useCount < _useLimit;

		bool IInteractable.CanInteract(PackedEntityWithWorld with) => CanInteract;

		void IInteractable.Begin(PackedEntityWithWorld with)
		{
			if (!with.Unpack(out var world, out var entity))
			{
				return;
			}

			_useCount++;
			AddComponents(world, entity);
			InteractionStarted?.Invoke();
		}

		//TODO: Сделать таймер
		void IInteractable.Execute(PackedEntityWithWorld with) { }

		void IInteractable.Finish(PackedEntityWithWorld with) => InteractionFinished?.Invoke();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddComponents(World world, Entity entity)
		{
			foreach (var database in _componentsToAdd)
			{
				foreach (var component in database.Components)
				{
					var componentType = component.GetType();
					if (!world.Pools.Contains(componentType))
					{
						continue;
					}

					var pool = world.Pool(componentType);
					if (!pool.Contains(entity))
					{
						pool.Allocate(entity, component);
					}
				}
			}
		}
	}
}