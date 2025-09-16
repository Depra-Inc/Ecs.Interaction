// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.QoL;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Depra.Ecs.Interaction
{
	public abstract class PickupItem : MonoBehaviour
	{
		[SerializeField] private UnityEvent _onCollected;

		public abstract bool Available(PackedEntityWithWorld @for);

		public void Collect(PackedEntityWithWorld @for)
		{
			Apply(@for);
			_onCollected.Invoke();
		}

		protected abstract void Apply(PackedEntityWithWorld entity);

		[PublicAPI]
		public void DestroySelf() => Destroy(gameObject);

		[PublicAPI]
		public void DestroySelfAfter(float delay) => Destroy(gameObject, delay);
	}
}