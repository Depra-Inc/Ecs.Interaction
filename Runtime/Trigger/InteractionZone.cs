// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using System.Collections.Generic;
using UnityEngine;
using static Depra.Ecs.Interaction.Trigger.TriggerInteractionModule;

namespace Depra.Ecs.Interaction.Trigger
{
	[RequireComponent(typeof(Collider))]
	[AddComponentMenu(MENU_PATH + nameof(InteractionZone))]
	public sealed class InteractionZone : MonoBehaviour
	{
		private readonly HashSet<InteractionZoneSensor> _sensors = new();

		public ITriggerInteractable Target { get; private set; }

		private void Awake() => Target = GetComponentInParent<ITriggerInteractable>();

		private void OnDisable()
		{
			foreach (var sensor in _sensors)
			{
				sensor.Release(this);
			}

			_sensors.Clear();
		}

		internal void Track(InteractionZoneSensor sensor) => _sensors.Add(sensor);

		internal void Untrack(InteractionZoneSensor sensor) => _sensors.Remove(sensor);

#if UNITY_EDITOR
		private void OnValidate() => GetComponent<Collider>().isTrigger = true;
#endif
	}
}
