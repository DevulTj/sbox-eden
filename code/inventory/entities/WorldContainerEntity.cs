// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

[Library( "eden_container" )]
public partial class WorldContainerEntity : Prop, IContainerEntity
{
	[Net]
	protected Container _Container { get; set; }
	// @IContainerEntity
	public Container Container { get => _Container; set => _Container = value; }

	public override void Spawn()
	{
		base.Spawn();

		Container = new();
		Container.Name = "Container";
	}
}
