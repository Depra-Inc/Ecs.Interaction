// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Health.Death;
using Depra.Ecs.QoL;
using Depra.Ecs.Transformation;
using UnityEngine;

namespace Depra.Ecs.Interaction
{
	public sealed class LootSpawnSystem : IInitializationSystem, IExecutionSystem
	{
		private IEntityQuery _entities;
		private LootDropAspect _lootDrop;
		private TransformationAspect _transform;

		void IInitializationSystem.Initialize(IWorldGroup worlds)
		{
			var world = worlds.Default;
			_entities = new EntityQueryBuilder()
				.With<Position>()
				.With<DeathEvent>()
				.With<LootPrefabsRef>()
				.Build().Initialize(world);

			_lootDrop = world.Aspect<LootDropAspect>();
			_transform = world.Aspect<TransformationAspect>();
		}

		void IExecutionSystem.Execute()
		{
			foreach (var entity in _entities)
			{
				var position = _transform.Position[entity].Value;
				var prefabs = _lootDrop.Prefabs[entity].Value;
				foreach (var prefab in prefabs)
				{
					var worldPosition = position + prefab.Offset;
					var worldRotation = prefab.Value.transform.rotation;
					Object.Instantiate(prefab.Value, worldPosition, worldRotation);
				}
			}
		}
	}
}