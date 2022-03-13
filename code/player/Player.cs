// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

public partial class Player : Sandbox.Player, IContainerEntity
{
	public Clothing.Container Clothing = new();

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

		Clothing.DressEntity( this );

		ResetVitals();

		Backpack = new();
		Backpack.SetSize( 28 );

		Hotbar = new( this );
		Hotbar.SetSize( 7 );
		Hotbar.Add( Item.FromAsset( "stone_hatchet" ) );
		Hotbar.SetActiveSlot( 0 );

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
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );
	}
}
