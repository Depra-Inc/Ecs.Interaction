// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Depra.Ecs.Interaction.Trigger
{
	public struct TriggerInteractionControlLock
	{
		private List<InputAction> _value;

		public List<InputAction> Value => _value ??= new List<InputAction>();
	}
}
