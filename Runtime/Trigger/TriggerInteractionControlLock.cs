// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Depra.Ecs.Interaction.Trigger
{
	public struct TriggerInteractionControlLock : IAutoReset<TriggerInteractionControlLock>
	{
		public List<InputAction> Value;

		void IAutoReset<TriggerInteractionControlLock>.AutoReset(ref TriggerInteractionControlLock component)
		{
			if (component.Value == null)
			{
				component.Value = new List<InputAction>();
			}
			else
			{
				component.Value.Clear();
			}
		}
	}
}
