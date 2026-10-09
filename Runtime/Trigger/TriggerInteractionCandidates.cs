// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using System.Collections.Generic;

namespace Depra.Ecs.Interaction.Trigger
{
	public struct TriggerInteractionCandidates : IAutoReset<TriggerInteractionCandidates>
	{
		public List<ITriggerInteractable> Value;

		void IAutoReset<TriggerInteractionCandidates>.AutoReset(ref TriggerInteractionCandidates component)
		{
			if (component.Value == null)
			{
				component.Value = new List<ITriggerInteractable>();
			}
			else
			{
				component.Value.Clear();
			}
		}
	}
}
