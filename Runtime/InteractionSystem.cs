// SPDX-License-Identifier: Apache-2.0
// © 2024 Evgeniy Kraevskiy <e.craevsky@depra.org>

using Depra.Ecs.Components;
using Depra.Ecs.QoL;
using UnityEngine;

namespace Depra.Ecs.Interaction
{
	public sealed class InteractionSystem : IInitializationSystem, IExecutionSystem
	{
		private World _world;
		private IEntityQuery _entities;

		private InteractionAspect _interaction;
		private ComponentPool<CameraRef> _camera;

		void IInitializationSystem.Initialize(IWorldGroup worlds)
		{
			_world = worlds.Default;

			// TODO: возможно обобщить, чтобы взамодействовать могли и другие объекты
			_entities = new EntityQueryBuilder()
				.With<CameraRef>()
				.With<InteractionLayerMask>()
				.With<InteractInputButton>()
				.With<InteractionDistance>()
				.Build().Initialize(_world);

			_camera = _world.Pool<CameraRef>();
			_interaction = _world.Aspect<InteractionAspect>();
		}

		void IExecutionSystem.Execute()
		{
			foreach (var entity in _entities)
			{
				// TODO: Использовать aim модуль
				var ray = _camera[entity].Value.ScreenPointToRay(Input.mousePosition);

				if (!Physics.Raycast(ray, out var hit, _interaction.Distance[entity].Value,
					    _interaction.Mask[entity].Value) ||
				    !hit.collider.TryGetComponent<IInteractable>(out var interactable))
				{
					continue;
				}

				if (Input.GetButtonDown(_interaction.Button[entity].Value))
				{
					interactable.Begin(_world.PackEntityWithWorld(entity));
				}
				else if (Input.GetButton(_interaction.Button[entity].Value))
				{
					interactable.Execute(_world.PackEntityWithWorld(entity));
				}
				else if (Input.GetButtonUp(_interaction.Button[entity].Value))
				{
					interactable.Finish(_world.PackEntityWithWorld(entity));
				}
			}
		}
	}
}