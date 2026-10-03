// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using System;
using UnityEngine.InputSystem;

namespace Depra.Ecs.Interaction.Trigger
{
	[Serializable]
	public struct TriggerInteractionControlLockConfig
	{
		public InputActionReference[] KeepEnabled;
	}
}
