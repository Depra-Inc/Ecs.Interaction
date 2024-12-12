// SPDX-License-Identifier: Apache-2.0
// © 2024 Evgeniy Kraevskiy <e.craevsky@depra.org>

using Depra.Ecs.QoL;
using UnityEngine;
using UnityEngine.Events;

namespace Depra.Ecs.Interaction
{
	internal sealed class ProxyInteractable : MonoBehaviour, IInteractable
	{
		[SerializeField] private UnityEvent _onBegin;
		[SerializeField] private UnityEvent _onFinish;

		[SerializeField] private int _useLimit = -1;
		private int _useCount = 0;

		public bool CanInteract => _useLimit == -1 || _useCount < _useLimit;

		void IInteractable.Begin(PackedEntityWithWorld sourceEntity)
		{
			_onBegin.Invoke();
			_useCount++;
		}

		void IInteractable.Execute(PackedEntityWithWorld sourceEntity)
		{
		}

		void IInteractable.Finish(PackedEntityWithWorld sourceEntity)
		{
			_onFinish.Invoke();
		}
	}
}