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
			obj.InteractionStarted += _onBegin.Invoke;
			obj.InteractionFinished += _onFinish.Invoke;
		}

		private void OnDisable()
		{
			var obj = GetComponent<InteractionObject>();
			obj.InteractionStarted -= _onBegin.Invoke;
			obj.InteractionFinished -= _onFinish.Invoke;
		}
	}
}