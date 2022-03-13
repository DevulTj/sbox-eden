// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;

namespace Eden;

[Library( "e_weapon" ), AutoGenerate]
public partial class WeaponItemAsset : ItemAsset
{
	public override ItemType Type => ItemType.Weapon;
	public override Color DefaultColor => Color.Blue;

	[Property]
	public string WeaponClassName { get; set; }
}
