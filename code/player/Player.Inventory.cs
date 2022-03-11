// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class Player
{
	[Net, Local]
	public Container Backpack { get; set; }
	// @IContainerEntity
	public Container Container { get => Backpack; set => Backpack = value; }
}
