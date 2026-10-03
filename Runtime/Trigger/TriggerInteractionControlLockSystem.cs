// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using Depra.Ecs.QoL;
using Depra.Logging;
using UnityEngine.InputSystem;

namespace Depra.Ecs.Interaction.Trigger
{
	public sealed class TriggerInteractionControlLockSystem : IInitializationSystem, IExecutionSystem
	{
		private EntityQuery _started;
		private EntityExclusionQuery _finished;
		private InteractionAspect _interaction;
		private TriggerInteractionAspect _trigger;

		void IInitializationSystem.Initialize(IWorldGroup worlds)
		{
			_started = new EntityQueryBuilder()
				.With<TriggerInteractionStartEvent>()
				.With<InteractInputActionRef>()
				.Build();

			_finished = new EntityQueryBuilder()
				.With<TriggerInteractionControlLock>()
				.Without<TriggerInteractionHold>()
				.Build();

			var world = worlds.Default;
			_started.Initialize(world);
			_finished.Initialize(world);
			_interaction = world.Aspect<InteractionAspect>();
			_trigger = world.Aspect<TriggerInteractionAspect>();
		}

		void IExecutionSystem.Execute()
		{
			foreach (var entity in _started)
			{
				Lock(entity);
			}

			foreach (var entity in _finished)
			{
				Unlock(entity);
			}
		}

		private void Lock(Entity entity)
		{
			var interact = _interaction.Button[entity].Value.action;
			var disabled = _trigger.ControlLock.GetOrAllocate(entity).Value;
			foreach (var action in interact.actionMap.actions)
			{
				if (action.enabled && action != interact && !IsKept(entity, action))
				{
					action.Disable();
					disabled.Add(action);
				}
			}

			_trigger.Logger.Info("Control locked ({0} actions)", disabled.Count);
		}

		private void Unlock(Entity entity)
		{
			var disabled = _trigger.ControlLock[entity].Value;
			foreach (var action in disabled)
			{
				action.Enable();
			}

			_trigger.Logger.Info("Control unlocked ({0} actions)", disabled.Count);
			_trigger.ControlLock.Delete(entity);
		}

		private bool IsKept(Entity entity, InputAction action)
		{
			if (!_trigger.ControlLockConfig.TryGet(entity, out var config) || config.KeepEnabled == null)
			{
				return false;
			}

			foreach (var kept in config.KeepEnabled)
			{
				if (kept && kept.action == action)
				{
					return true;
				}
			}

			return false;
		}
	}
}
