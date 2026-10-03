// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using System;
using System.Collections.Generic;

namespace Depra.Ecs.Interaction.Trigger
{
	[Serializable]
	public struct TriggerInteractionCandidates
	{
		private List<ITriggerInteractable> _value;

		public List<ITriggerInteractable> Value => _value ??= new List<ITriggerInteractable>();
	}
}
