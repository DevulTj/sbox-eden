// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.ComponentModel;

namespace Eden;

[Library( "e_food" ), AutoGenerate]
public partial class ConsumableItemAsset : ItemAsset
{
	public override ItemType Type => ItemType.Consumable;
	public override Color DefaultColor => Color.Green;

	[Property, Category( "Consumable" ), Range( 0, 500 )]
	public int Hunger { get; set; } = 0;

	[Property, Category( "Consumable" ), Range( 0, 300 )]
	public int Thirst { get; set; } = 0;
}
