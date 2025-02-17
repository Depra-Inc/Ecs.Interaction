// SPDX-License-Identifier: Apache-2.0
// © 2025 Depra <n.melnikov@depra.org>

using Depra.Ecs.Modules;

namespace Depra.Ecs.Interaction
{
	public sealed class PickUpCollectionModule : IModule
	{
		internal const string MENU_PATH = nameof(Ecs) + "/" + nameof(Interaction) + "/";

		IComponentAspect[] IModule.Aspects => new IComponentAspect[]
		{
			new PickUpCollectionAspect()
		};

		void IModule.Initialize(ISystemGroup systems) => systems.Add(new PickUpCollectionSystem());
	}
}