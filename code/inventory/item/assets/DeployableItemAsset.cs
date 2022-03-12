// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;

namespace Eden;

[Library( "e_deploy" ), AutoGenerate]
public partial class DeployableItemAsset : ItemAsset
{
	public override ItemType Type => ItemType.Deployable;
	public override Color DefaultColor => Color.Red;

	[Property]
	public string EntityClassName { get; set; }
}
