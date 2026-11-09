// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using System.Collections.Generic;
using Depra.Ecs.Components;
using Depra.Ecs.QoL;
using Depra.Ecs.Transformation;
using UnityEngine;

namespace Depra.Ecs.Interaction.Trigger
{
	public sealed class TriggerInteractionFocusSystem : IInitializationSystem, IExecutionSystem
	{
		private EntityExclusionQuery _entities;
		private TriggerInteractionAspect _trigger;
		private ComponentPool<TransformRef> _transform;

		void IInitializationSystem.Initialize(IWorldGroup worlds)
		{
			_entities = new EntityQueryBuilder()
				.With<TransformRef>()
				.With<TriggerInteractionCandidates>()
				.Without<TriggerInteractionHold>()
				.Build();

			var world = worlds.Default;
			_entities.Initialize(world);
			_transform = world.Pool<TransformRef>();
			_trigger = world.Aspect<TriggerInteractionAspect>();
		}

		void IExecutionSystem.Execute()
		{
			foreach (var entity in _entities)
			{
				var position = _transform[entity].Value.position;
				var target = FindNearest(_trigger.Candidates[entity].Value, position);
				if (target == null)
				{
					_trigger.Focus.TryDelete(entity);
					continue;
				}

				_trigger.Focus.GetOrAllocate(entity).Target = target;
			}
		}

		private ITriggerInteractable FindNearest(List<ITriggerInteractable> candidates, Vector3 position)
		{
			ITriggerInteractable nearest = null;
			var nearestDistance = float.PositiveInfinity;
			foreach (var candidate in candidates)
			{
				var anchor = candidate.Anchor;
				var distance = anchor ? (anchor.position - position).sqrMagnitude : float.PositiveInfinity;
				if (nearest == null || distance < nearestDistance)
				{
					nearest = candidate;
					nearestDistance = distance;
				}
			}

			return nearest;
		}
	}
}
