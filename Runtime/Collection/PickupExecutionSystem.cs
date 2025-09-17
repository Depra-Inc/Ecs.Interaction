// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.QoL;

namespace Depra.Ecs.Interaction
{
	public sealed class PickupExecutionSystem : IInitializationSystem, IExecutionSystem
	{
		private World _world;
		private IEntityQuery _entities;
		private ComponentPool<PickupRequest> _request;

		void IInitializationSystem.Initialize(IWorldGroup worlds)
		{
			_world = worlds.Default;
			_entities = new EntityQueryBuilder()
				.With<PickupRequest>()
				.Build().Initialize(_world);

			_request = _world.Pool<PickupRequest>();
		}

		void IExecutionSystem.Execute()
		{
			foreach (var entity in _entities)
			{
				var packedEntity = _world.PackEntityWithWorld(entity);
				var item = _request[entity].Value;
				if (item.Available(@for: packedEntity))
				{
					item.Collect(@for: packedEntity);
				}

				_request.Delete(entity);
			}
		}
	}
}