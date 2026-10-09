// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using System.Collections.Generic;
using Depra.Ecs.Hybrid;
using Depra.Ecs.QoL;
using Depra.Logging;
using UnityEngine;
using static Depra.Ecs.Interaction.Trigger.TriggerInteractionModule;

namespace Depra.Ecs.Interaction.Trigger
{
	[DisallowMultipleComponent]
	[AddComponentMenu(MENU_PATH + nameof(InteractionZoneSensor))]
	internal sealed class InteractionZoneSensor : MonoBehaviour, IAuthoring
	{
		private readonly Dictionary<InteractionZone, int> _overlaps = new();

		private PackedEntityWithWorld _entity = PackedEntityWithWorld.NULL;
		private TriggerInteractionAspect _trigger;
		private ComponentPool<TriggerInteractionCandidates> _candidates;

		IBaker IAuthoring.CreateBaker() => new Baker(this);

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent<InteractionZone>(out var zone) || zone.Target == null)
			{
				return;
			}

			_overlaps.TryGetValue(zone, out var count);
			_overlaps[zone] = count + 1;
			if (count > 0)
			{
				return;
			}

			zone.Track(this);
			if (_entity.Unpack(out _, out var entity))
			{
				_candidates.GetOrAllocate(entity).Value.Add(zone.Target);
				_trigger.Logger.Info("Enter zone {0}", zone.Target);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent<InteractionZone>(out var zone) ||
			    !_overlaps.TryGetValue(zone, out var count))
			{
				return;
			}

			if (count > 1)
			{
				_overlaps[zone] = count - 1;
				return;
			}

			zone.Untrack(this);
			Release(zone);
		}

		private void OnDisable()
		{
			foreach (var zone in _overlaps.Keys)
			{
				zone.Untrack(this);
			}

			_overlaps.Clear();
			if (_entity.Unpack(out _, out var entity) && _candidates.TryGet(entity, out var candidates))
			{
				candidates.Value.Clear();
			}
		}

		internal void Release(InteractionZone zone)
		{
			_overlaps.Remove(zone);
			if (_entity.Unpack(out _, out var entity) && _candidates.TryGet(entity, out var candidates))
			{
				candidates.Value.Remove(zone.Target);
				_trigger.Logger.Info("Exit zone {0}", zone.Target);
			}
		}

		private readonly struct Baker : IBaker
		{
			private readonly InteractionZoneSensor _sensor;

			public Baker(InteractionZoneSensor sensor) => _sensor = sensor;

			void IBaker.Bake(IAuthoring authoring, World world)
			{
				if (!((IAuthoringEntity)authoring).Unpack(out var entity))
				{
					return;
				}

				_sensor._entity = world.PackEntityWithWorld(entity);
				_sensor._trigger = world.Aspect<TriggerInteractionAspect>();
				_sensor._candidates = world.Pool<TriggerInteractionCandidates>();
			}
		}
	}
}
