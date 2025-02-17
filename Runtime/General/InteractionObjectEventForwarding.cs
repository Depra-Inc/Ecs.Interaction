// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Depra.Ecs.Interaction
{
	[RequireComponent(typeof(InteractionObject))]
	internal sealed class InteractiveObjectEventForwarding : MonoBehaviour
	{
		[SerializeField] private UnityEvent _onBegin;
		[SerializeField] private UnityEvent _onFinish;

		private void OnEnable()
		{
			var obj = GetComponent<InteractionObject>();
			obj.Touched += _onBegin.Invoke;
			obj.Completed += _onFinish.Invoke;
		}

		private void OnDisable()
		{
			var obj = GetComponent<InteractionObject>();
			obj.Touched -= _onBegin.Invoke;
			obj.Completed -= _onFinish.Invoke;
		}

		[PublicAPI]
		public void DestroyInRuntime() => Destroy(gameObject);
	}
}