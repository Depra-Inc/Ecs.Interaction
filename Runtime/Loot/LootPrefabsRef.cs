// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using UnityEngine;

namespace Depra.Ecs.Interaction
{
	[System.Serializable]
	public struct LootPrefabsRef
	{
		public Prefab[] Value;

		[System.Serializable]
		public struct Prefab
		{
			public Vector3 Offset;
			public GameObject Value;
			[Range(0, 1)] public float Chance;
		}
	}
}