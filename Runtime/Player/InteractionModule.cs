// SPDX-License-Identifier: Apache-2.0
// © 2024 Depra <e.craevsky@depra.org>
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Modules;

namespace Depra.Ecs.Interaction
{
	public sealed class InteractionModule : IModule
	{
		IComponentAspect[] IModule.Aspects => new IComponentAspect[]
		{
			new InteractionAspect()
		};

		void IModule.Initialize(ISystemGroup systems) => systems.Add(new InteractionAimSystem());
	}
}