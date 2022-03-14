// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

[Library( "eden_resource", Spawnable = true )]
public partial class ResourceEntity : Prop
{
	[Net]
	public ResourceType Type { get; set; } = ResourceType.Wood;

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

		Log.Info( info.Weapon );
	}
}
