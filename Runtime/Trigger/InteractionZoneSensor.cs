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
	internal sealed class InteractionZoneSensor : MonoBehaviour
	{
		[SerializeField] private AuthoringEntity _authoringEntity;

		private readonly Dictionary<InteractionZone, int> _overlaps = new();

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
			if (_authoringEntity.Unpack(out var world, out var entity))
			{
				world.Pool<TriggerInteractionCandidates>().GetOrAllocate(entity).Value.Add(zone.Target);
				world.Aspect<TriggerInteractionAspect>().Logger.Info("Enter zone {0}", zone.Target);
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
			if (_authoringEntity.Unpack(out var world, out var entity) &&
			    world.Pool<TriggerInteractionCandidates>().TryGet(entity, out var candidates))
			{
				candidates.Value.Clear();
			}
		}

		internal void Release(InteractionZone zone)
		{
			_overlaps.Remove(zone);
			if (_authoringEntity.Unpack(out var world, out var entity) &&
			    world.Pool<TriggerInteractionCandidates>().TryGet(entity, out var candidates))
			{
				candidates.Value.Remove(zone.Target);
				world.Aspect<TriggerInteractionAspect>().Logger.Info("Exit zone {0}", zone.Target);
			}
		}
	}
}
