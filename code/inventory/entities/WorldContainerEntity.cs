// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class WorldContainerEntity : Prop, IContainerEntity
{
	[Net, Local]
	protected Container _Container { get; set; }
	// @IContainerEntity
	public Container Container { get => _Container; set => _Container = value; }
}
