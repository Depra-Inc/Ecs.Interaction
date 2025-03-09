// SPDX-License-Identifier: Apache-2.0
// © 2024 Depra <e.craevsky@depra.org>
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Components;
using Depra.Ecs.QoL;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Depra.Ecs.Interaction
{
	public sealed class InteractionAimSystem : IInitializationSystem, IExecutionSystem
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
				.With<InteractInputActionRef>()
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
				var layerMask = _interaction.Mask[entity].Value;
				var maxDistance = _interaction.Distance[entity].Value;
				var ray = _camera[entity].Value.ScreenPointToRay(Mouse.current.position.value);
				if (!Physics.Raycast(ray, out var hit, maxDistance, layerMask) ||
				    !hit.collider.TryGetComponent<IInteractable>(out var interactable))
				{
					continue;
				}

				var inputAction = _interaction.Button[entity].Value.action;
				if (inputAction.WasPressedThisFrame())
				{
					interactable.Touch(_world.PackEntityWithWorld(entity));
				}
				else if (inputAction.WasPressedThisFrame())
				{
					interactable.Process(_world.PackEntityWithWorld(entity));
				}
				else if (inputAction.WasReleasedThisFrame())
				{
					interactable.Complete(_world.PackEntityWithWorld(entity));
				}
			}
		}
	}
}