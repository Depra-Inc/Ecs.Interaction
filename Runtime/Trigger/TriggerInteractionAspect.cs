// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using Depra.Logging;

namespace Depra.Ecs.Interaction.Trigger
{
	public sealed class TriggerInteractionAspect : IComponentAspect
	{
		public TriggerInteractionAspect(Logger logger) => Logger = logger;

		public Logger Logger { get; }
		public ComponentPool<TriggerInteractionCandidates> Candidates { get; private set; }
		public ComponentPool<TriggerInteractionFocus> Focus { get; private set; }
		public ComponentPool<TriggerInteractionHold> Hold { get; private set; }
		public ComponentPool<TriggerInteractionAligning> Aligning { get; private set; }
		public ComponentPool<TriggerInteractionControlLock> ControlLock { get; private set; }
		public ComponentPool<TriggerInteractionControlLockConfig> ControlLockConfig { get; private set; }
		public ComponentPool<TriggerInteractionStartEvent> StartEvent { get; private set; }
		public ComponentPool<TriggerInteractionBeginEvent> BeginEvent { get; private set; }
		public ComponentPool<TriggerInteractionCancelEvent> CancelEvent { get; private set; }
		public ComponentPool<TriggerInteractionCompleteEvent> CompleteEvent { get; private set; }

		void IComponentAspect.Initialize(AspectGroup aspects, ComponentPoolGroup pools)
		{
			aspects.Add(this);
			pools.Add(Candidates = new ComponentPool<TriggerInteractionCandidates>());
			pools.Add(Focus = new ComponentPool<TriggerInteractionFocus>());
			pools.Add(Hold = new ComponentPool<TriggerInteractionHold>());
			pools.Add(Aligning = new ComponentPool<TriggerInteractionAligning>());
			pools.Add(ControlLock = new ComponentPool<TriggerInteractionControlLock>());
			pools.Add(ControlLockConfig = new ComponentPool<TriggerInteractionControlLockConfig>());
			pools.Add(StartEvent = new ComponentPool<TriggerInteractionStartEvent>());
			pools.Add(BeginEvent = new ComponentPool<TriggerInteractionBeginEvent>());
			pools.Add(CancelEvent = new ComponentPool<TriggerInteractionCancelEvent>());
			pools.Add(CompleteEvent = new ComponentPool<TriggerInteractionCompleteEvent>());
		}
	}
}
