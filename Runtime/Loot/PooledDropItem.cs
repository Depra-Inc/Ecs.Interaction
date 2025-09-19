// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Pooling;
using UnityEngine;

namespace Depra.Ecs.Interaction
{
	[DisallowMultipleComponent]
	internal sealed class PooledDropItem : PooledGameObject
	{
		private int _poolId;
		private PoolService _service;

		public void Initialize(int poolId, PoolService service)
		{
			_poolId = poolId;
			_service = service;
		}

		public void OnCollected()
		{
			gameObject.SetActive(false);
			_service.Release(_poolId, this);
		}

		public override void OnPoolReuse() => gameObject.SetActive(true);
	}
}