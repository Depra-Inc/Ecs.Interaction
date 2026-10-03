// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using Depra.Ecs.Modules;
using Depra.Ecs.QoL;
using Depra.Ecs.Timeout;
using Depra.IoC.Injection.Attributes;
using Depra.Logging;

namespace Depra.Ecs.Interaction.Trigger
{
	public sealed class TriggerInteractionModule : IModule
	{
		internal const string MENU_PATH = "Ecs/Interaction/Trigger/";
		private const string LOG_CHANNEL = "TriggerInteraction";

		[Dependency] private readonly Logger _logger;

		IComponentAspect[] IModule.Aspects => new IComponentAspect[]
		{
			new TriggerInteractionAspect(_logger.Channel(LOG_CHANNEL)),
			new TimeoutAspect<TriggerInteractionHold>()
		};

		void IModule.Initialize(ISystemGroup systems) => systems
			.Add(new TriggerInteractionFocusSystem())
			.Add(new TimeoutSystems<TriggerInteractionHold>())
			.Add(new TriggerInteractionHoldSystem())
			.Add(new TriggerInteractionControlLockSystem())
			.Add(new ComponentDeletionSystem<TriggerInteractionStartEvent>(), SystemOrder.LAST)
			.Add(new ComponentDeletionSystem<TriggerInteractionBeginEvent>(), SystemOrder.LAST)
			.Add(new ComponentDeletionSystem<TriggerInteractionCancelEvent>(), SystemOrder.LAST)
			.Add(new ComponentDeletionSystem<TriggerInteractionCompleteEvent>(), SystemOrder.LAST);
	}
}
