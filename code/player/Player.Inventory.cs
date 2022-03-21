// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;

namespace Eden;

public partial class Player
{
	[Net, Local, Change( nameof( OnBackpackChanged ) )]
	public Container Backpack { get; protected set; }
	// @IContainerEntity
	public Container Container { get => Backpack; set => Backpack = value; }

	[Net, Local, Change( nameof( OnHotbarChanged ) )]
	public HotbarContainer Hotbar { get; protected set; }

	public Container[] Containers => new Container[] { Backpack, Container };

	protected void OnHotbarChanged( HotbarContainer old, HotbarContainer @new )
	{
		Log.Info( "Hotbar Container flagged as different by the server" );
		Event.Run( GameEvent.Client.HotbarChanged, @new );
	}

	protected void OnBackpackChanged( Container old, Container @new )
	{
		Log.Info( "Backpack Container flagged as different by the server" );
		Event.Run( GameEvent.Client.BackpackChanged, @new );
	}

	public void SetupInventory()
	{
		Backpack = new( this );
		Backpack.SetSize( 28 );
		Backpack.Name = "Backpack";

		Hotbar = new( this );
		Hotbar.SetSize( 7 );
		Hotbar.Name = "Equipment";

		Hotbar.Add( Item.FromAsset( "stone_hatchet" ), true );
		Hotbar.Add( Item.FromAsset( "stone_pickaxe" ), false );
	}

	public void HotbarSimulate()
	{
		int desiredSlot = -1;

		if ( Input.Pressed( InputButton.Slot1 ) ) desiredSlot = 0;
		if ( Input.Pressed( InputButton.Slot2 ) ) desiredSlot = 1;
		if ( Input.Pressed( InputButton.Slot3 ) ) desiredSlot = 2;
		if ( Input.Pressed( InputButton.Slot4 ) ) desiredSlot = 3;
		if ( Input.Pressed( InputButton.Slot5 ) ) desiredSlot = 4;
		if ( Input.Pressed( InputButton.Slot6 ) ) desiredSlot = 5;
		if ( Input.Pressed( InputButton.Slot7 ) ) desiredSlot = 6;
		if ( Input.Pressed( InputButton.Slot8 ) ) desiredSlot = 7;

		if ( desiredSlot == -1 )
			return;

		Hotbar.SetActiveSlot( desiredSlot );
	}
}
