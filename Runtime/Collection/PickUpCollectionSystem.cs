// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.QoL;

namespace Depra.Ecs.Interaction
{
	public sealed class PickUpCollectionSystem : IInitializationSystem, IExecutionSystem
	{
		private World _world;
		private IEntityQuery _entities;
		private PickUpCollectionAspect _collectablePick;

		void IInitializationSystem.Initialize(IWorldGroup worlds)
		{
			_world = worlds.Default;
			_entities = new EntityQueryBuilder()
				.With<PickUpRequest>()
				.Build().Initialize(_world);

			_collectablePick = _world.Aspect<PickUpCollectionAspect>();
		}

		void IExecutionSystem.Execute()
		{
			foreach (var entity in _entities)
			{
				var packedEntity = _world.PackEntityWithWorld(entity);
				var interactable = _collectablePick.UpRequest[entity].Value;
				if (interactable.CanInteract(with: packedEntity))
				{
					interactable.Touch(packedEntity);
					interactable.Complete(packedEntity);
				}
			}
		}
	}
}