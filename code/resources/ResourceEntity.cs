// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;

namespace Eden;

[Library( "eden_resource", Spawnable = true )]
public partial class ResourceEntity : Prop
{
	// @TODO: need a better way to do this, I don't like it
	public static Dictionary<ResourceType, string> ResourceMap = new()
	{
		{ ResourceType.Wood, "wooden_plank" },
		{ ResourceType.Stone, "stone" },
	};

	[Net]
	public ResourceType Type { get; set; } = ResourceType.Wood;

	[Net]
	public int ResourceAmount { get; set; } = 1000;

	public static Model BaseModel { get; set; } = Model.Load( "models/resources/resource_blockout.vmdl" );

	public override void Spawn()
	{
		base.Spawn();

		Model = BaseModel;
		MoveType = MoveType.None;
	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );

		var weapon = info.Weapon as MeleeWeapon;
		var player = info.Attacker as Player;

		if ( player is null )
			return;

		if ( weapon is null )
			return;

		var resourceYield = weapon.GetResourceYield( Type );

		if ( resourceYield < 1 )
			return;

		var assetName = ResourceMap[Type];
		var item = Item.FromAsset( assetName );

		ResourceAmount -= resourceYield;

		var grantedAmount = resourceYield;
		if ( ResourceAmount < 0 )
			grantedAmount += ResourceAmount;

		if ( !InventoryHelpers.GiveItem( player, item, resourceYield ) )
		{
			ResourceNotifications.AddResource( To.Single( player.Client ), grantedAmount, item.Asset.ItemName );

			var entity = WorldItemEntity.Instantiate( item, grantedAmount );
			entity.Position = Position + Vector3.Up * 10f;
		}
	}
}
