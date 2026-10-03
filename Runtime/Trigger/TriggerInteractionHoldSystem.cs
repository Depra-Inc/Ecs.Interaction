// SPDX-License-Identifier: Apache-2.0
// © 2026 Depra <e.craevsky@depra.org>

using Depra.Ecs.QoL;
using Depra.Ecs.Timeout;
using Depra.Logging;
using UnityEngine.InputSystem;

namespace Depra.Ecs.Interaction.Trigger
{
	public sealed class TriggerInteractionHoldSystem : IInitializationSystem, IExecutionSystem
	{
		private World _world;
		private EntityQuery _entities;
		private InteractionAspect _interaction;
		private TriggerInteractionAspect _trigger;
		private TimeoutAspect<TriggerInteractionHold> _timeout;
		private ComponentPool<ElapsedTime<TriggerInteractionHold>> _elapsed;

		void IInitializationSystem.Initialize(IWorldGroup worlds)
		{
			_entities = new EntityQueryBuilder()
				.With<InteractInputActionRef>()
				.With<TriggerInteractionCandidates>()
				.Build();

			_world = worlds.Default;
			_entities.Initialize(_world);
			_interaction = _world.Aspect<InteractionAspect>();
			_trigger = _world.Aspect<TriggerInteractionAspect>();
			_timeout = _world.Timeout<TriggerInteractionHold>();
			_elapsed = _world.Pool<ElapsedTime<TriggerInteractionHold>>();
		}

		void IExecutionSystem.Execute()
		{
			foreach (var entity in _entities)
			{
				var action = _interaction.Button[entity].Value.action;
				var with = _world.PackEntityWithWorld(entity);
				if (_trigger.Hold.Contains(entity))
				{
					Tick(entity, with, action);
				}
				else if (action.WasPressedThisFrame() &&
				         _trigger.Focus.TryGet(entity, out var focus) &&
				         focus.Target.CanInteract(with))
				{
					Start(entity, focus.Target);
					Tick(entity, with, action);
				}
			}
		}

		private void Start(Entity entity, ITriggerInteractable target)
		{
			_trigger.Hold.Allocate(entity) = new TriggerInteractionHold { Target = target };
			_trigger.StartEvent.Allocate(entity).Target = target;
			bool aligning = target.Anchor;
			if (aligning)
			{
				_trigger.Aligning.Allocate(entity);
			}

			_trigger.Logger.Info("Start {0} (aligning: {1})", target, aligning);
		}

		private void Tick(Entity entity, PackedEntityWithWorld with, InputAction action)
		{
			ref var hold = ref _trigger.Hold[entity];
			var target = hold.Target;
			if (hold.Begun && _timeout.EndEvent.Contains(entity))
			{
				Finish(entity);
				_trigger.CompleteEvent.Allocate(entity).Target = target;
				target.Complete(with);
				_trigger.Logger.Info("Complete {0}", target);
				return;
			}

			var cancelReason = FindCancelReason(entity, with, action, target);
			if (cancelReason != null)
			{
				Finish(entity);
				_trigger.CancelEvent.Allocate(entity).Target = target;
				target.Cancel(with);
				_trigger.Logger.Info("Cancel {0} ({1})", target, cancelReason);
				return;
			}

			if (_trigger.Aligning.Contains(entity))
			{
				return;
			}

			if (!hold.Begun)
			{
				hold.Begun = true;
				_timeout.Start(entity, target.Duration);
				_trigger.BeginEvent.Allocate(entity).Target = target;
				target.Begin(with);
				_trigger.Logger.Info("Begin {0} ({1}: {2}s)", target, target.Mode, target.Duration);
				return;
			}

			_timeout.GetTimeout(entity, out var elapsed, out var duration);
			if (duration > 0f)
			{
				target.Progress(with, elapsed / duration);
			}
		}

		private string FindCancelReason(Entity entity, PackedEntityWithWorld with, InputAction action,
			ITriggerInteractable target)
		{
			if (target.Mode == TriggerInteractionMode.Hold && !action.IsPressed())
			{
				return "released";
			}

			if (!_trigger.Candidates[entity].Value.Contains(target))
			{
				return "left zone";
			}

			return target.CanInteract(with) ? null : "cannot interact";
		}

		private void Finish(Entity entity)
		{
			_trigger.Hold.Delete(entity);
			_trigger.Aligning.TryDelete(entity);
			_elapsed.TryDelete(entity);
			_timeout.StartRequests.TryDelete(entity);
		}
	}
}
