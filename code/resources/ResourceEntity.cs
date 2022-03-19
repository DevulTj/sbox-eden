// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;

namespace Eden;

[Library( "eden_resource_single", Spawnable = true )]
public partial class ResourceEntity : ModelEntity
{
	protected static Model BaseModel { get; set; } = Model.Load( "models/items/sticks/stick_small.vmdl" );

	public event Action<ResourceEntity> OnDestroyed;

	public TimeSince LastRefresh { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		Model = BaseModel;
		MoveType = MoveType.None;
		CollisionGroup = CollisionGroup.Debris;
		PhysicsEnabled = false;
		UsePhysicsCollision = false;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		OnDestroyed?.Invoke( this );
	}
}
