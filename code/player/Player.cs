// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

public partial class Player : Sandbox.Player, IContainerEntity
{
	public Clothing.Container Clothing = new();

	protected DamageInfo LastDamageInfo { get; set; }

	public Player()
	{
		//
	}

	public Player( Client cl ) : base()
	{
		Clothing.LoadFromClient( cl );
	}

	public override void Spawn()
	{
		base.Spawn();

		SetupVitals();
	}

	public void SetupInventory()
	{
		Backpack = new();
		Backpack.SetSize( 28 );

		Hotbar = new( this );
		Hotbar.SetSize( 7 );
		Hotbar.Add( Item.FromAsset( "stone_hatchet" ), true );
	}

	public override void Respawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();
		CameraMode = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		CraftingQueue = new( this );

		Clothing.DressEntity( this );

		ResetVitals();

		SetupInventory();

		base.Respawn();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( LifeState != LifeState.Alive )
			return;

		TickPlayerUse();
		SimulateActiveChild( cl, ActiveChild );
		HotbarSimulate();
		TickVitals();

		if ( Input.Released( InputButton.View ) )
		{
			if ( CameraMode is FirstPersonCamera )
			{
				CameraMode = new ThirdPersonCamera();
			}
			else
			{
				CameraMode = new FirstPersonCamera();
			}
		}
	}

	public override void TakeDamage( DamageInfo info )
	{
		// @TODO: damage system
		if ( GetHitboxGroup( info.HitboxIndex ) == 1 )
		{
			info.Damage *= 2.0f;
		}

		LastDamageInfo = info;

		base.TakeDamage( info );
	}

	public override void OnKilled()
	{
		base.OnKilled();

		BecomeRagdollOnClient( Velocity, LastDamageInfo.Flags, LastDamageInfo.Position, LastDamageInfo.Force, GetHitboxBone( LastDamageInfo.HitboxIndex ) );

		Controller = null;

		EnableAllCollisions = false;
		EnableDrawing = false;

		CameraMode = new SpectateRagdollCamera();

		foreach ( var child in Children )
		{
			child.EnableDrawing = false;
		}
	}
}
