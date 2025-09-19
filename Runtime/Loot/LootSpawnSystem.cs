// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using System.Runtime.CompilerServices;
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
				.With<LootDropPresetRef>()
				.Build().Initialize(world);

			_lootDrop = world.Aspect<LootDropAspect>();
			_transform = world.Aspect<TransformationAspect>();
		}

		void IExecutionSystem.Execute()
		{
			foreach (var entity in _entities)
			{
				var preset = _lootDrop.Preset[entity].Value;
				var position = _transform.Position[entity].Value;

				foreach (var item in preset.AllItems())
				{
					if (Random.value < item.Chance)
					{
						SpawnType(preset, item, position);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SpawnType(DropPreset preset, DropPreset.DropItem item, Vector3 position)
		{
			for (var j = 0; j < item.Count; j++)
			{
				var randomCircle = Random.insideUnitCircle * preset.Radius;
				var spawnOffset = new Vector3(randomCircle.x, item.VerticalOffset, randomCircle.y);
				var spawnPosition = position + spawnOffset;

				var poolId = item.Prefab.GetInstanceID();
				var droppedItem = _lootDrop.Pooling.Request<PooledDropItem>(poolId);
				droppedItem.transform.SetPositionAndRotation(spawnPosition, Random.rotation);
				droppedItem.Initialize(poolId, _lootDrop.Pooling);

				if (droppedItem.TryGetComponent<Rigidbody>(out var rigidbody))
				{
					var horizontalForce = Random.Range(item.HorizontalForceMin, item.HorizontalForceMax);
					var horizontalDirection = new Vector3(
						Random.Range(-1f, 1f),
						0,
						Random.Range(-1f, 1f)).normalized;

					var jumpForce = horizontalDirection * horizontalForce;
					jumpForce.y = Random.Range(item.UpwardForceMin, item.UpwardForceMax);
					rigidbody.AddForce(jumpForce, ForceMode.Impulse);
					rigidbody.angularVelocity = Random.insideUnitSphere * 5f;
				}
			}
		}
	}
}