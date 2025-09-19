using Depra.Pooling;
using UnityEngine;

namespace Depra.Ecs.Interaction
{
	[CreateAssetMenu(fileName = FILE_NAME, menuName = "Depra/Ecs/Interaction/" + FILE_NAME, order = 52)]
	public sealed class DropPreset : ScriptableObject
	{
		[SerializeField] private DropItem[] _possibleItems;
		[Min(0)] [SerializeField] private float _radius;

		private const string FILE_NAME = "Drop Preset";

		public float Radius => _radius;
		public DropItem[] AllItems() => _possibleItems;

		[System.Serializable]
		public class DropItem
		{
			[SerializeField] private PooledGameObject _prefab;
			[SerializeField] [Min(0)] private int _count;
			[Range(0, 1)] [SerializeField] private float _chance;

			[SerializeField] private float _verticalOffset = 0.5f;
			[SerializeField] private float _upwardForceMin = 2f;
			[SerializeField] private float _upwardForceMax = 5f;
			[SerializeField] private float _horizontalForceMin = 2f;
			[SerializeField] private float _horizontalForceMax = 5f;

			public int Count => _count;
			public float Chance => _chance;
			public PooledGameObject Prefab => _prefab;

			public float VerticalOffset => _verticalOffset;
			public float UpwardForceMin => _upwardForceMin;
			public float UpwardForceMax => _upwardForceMax;
			public float HorizontalForceMin => _horizontalForceMin;
			public float HorizontalForceMax => _horizontalForceMax;
		}
	}
}