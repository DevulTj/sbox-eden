// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;

namespace Eden;

[Library( "e_deploy" ), AutoGenerate]
public partial class DeployableItemAsset : ItemAsset
{
	[Property, Range( 0, 500 )]
	public int Hunger { get; set; } = 0;

	[Property, Range( 0, 300 )]
	public int Thirst { get; set; } = 0;
}
