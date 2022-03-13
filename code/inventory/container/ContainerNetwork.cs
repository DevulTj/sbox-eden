// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;

namespace Eden;

public partial class ContainerNetwork
{
	public static Dictionary<Guid, Container> Registry { get; protected set; } = new();

	public static Container Get( Guid guid )
	{
		if ( Registry.ContainsKey( guid ) )
			return Registry[guid];

		return null;
	}

	public static bool Register( Container container )
	{
		var guid = Guid.NewGuid();
		container.ID = guid;

		return Registry.TryAdd( guid, container );
	}

	public static bool Dispose( Container container )
	{
		return Registry.Remove( container.ID );
	}


	[ServerCmd( "eden_container_move" )]
	public static void ContainerMove( string guidString, int slotA, int slotB )
	{
		var guid = Guid.Parse( guidString );
		var container = Get( guid );

		var player = ConsoleSystem.Caller.Pawn as Player;
		// @TODO: validate this so it's not abused by players
		// We will need a verification method on containers
		// like container.CanInteract( player )

		if ( container is null )
			return;

		Log.Info( $"container: {guidString}, slotA: {slotA}, slotB: {slotB}" );

		container.Move( slotA, slotB );

		UpdatePlayer( To.Single( player.Client ), guidString );
	}

	[ServerCmd( "eden_container_move_external" )]
	public static void ContainerMoveExternal( string guidString, int slotA, string destinationGuidString, int slotB )
	{
		var guid = Guid.Parse( guidString );
		var destinationGuid = Guid.Parse( destinationGuidString );
		var container = Get( guid );
		var destinationContainer = Get( destinationGuid );

		var player = ConsoleSystem.Caller.Pawn as Player;
		// @TODO: validate this so it's not abused by players
		// We will need a verification method on containers
		// like container.CanInteract( player )

		if ( container is null )
			return;

		Log.Info( $"container: {guidString}, slotA: {slotA}, slotB: {slotB}" );

		container.Move( slotA, slotB, destinationContainer );

		UpdatePlayer( To.Single( player.Client ), guidString );
		UpdatePlayer( To.Single( player.Client ), destinationGuidString );
	}

	public static void PickupItem( Player player, WorldItemEntity worldItem )
	{
		var container = player.Backpack;
		container.Add( worldItem.Item, worldItem.Quantity );

		UpdatePlayer( To.Single( player.Client ), container.ID.ToString() );

		worldItem.Delete();
	}

	[ServerCmd( "eden_container_drop" )]
	public static void ContainerDrop( string guidString, int slotA )
	{
		var guid = Guid.Parse( guidString );
		var container = Get( guid );

		var player = ConsoleSystem.Caller.Pawn as Player;
		// @TODO: validate this so it's not abused by players
		// We will need a verification method on containers
		// like container.CanInteract( player )

		if ( container is null )
			return;

		var slot = container.GetSlot( slotA );
		if ( slot is null || slot.Item is null )
			return;

		var entity = WorldItemEntity.InstantiateFromPlayer( player, slot.Item, slot.Quantity );

		// @TODO: unfuck this later
		container.Remove( slotA );

		Log.Info( $"{entity} was spawned as a result of dropping an item" );

		UpdatePlayer( To.Single( player.Client ), guidString );
	}

	[ClientRpc]
	public static void UpdatePlayer( string guid )
	{
		var matchingPanels = ContainerPanel.GetFromID( Guid.Parse( guid ) );

		if ( matchingPanels is not null && matchingPanels.Count > 0 )
		{
			matchingPanels.ForEach( x => x.Refresh() );
		}
	}

	[AdminCmd( "eden_admin_spawnitem" )]
	public static void SpawnItem( string assetClass, int quantity = 1 )
	{
		var item = Item.FromAsset( assetClass );
		if ( item is not null )
		{
			var player = ConsoleSystem.Caller.Pawn as Player;

			WorldItemEntity.InstantiateFromPlayer( player, item, quantity );
		}
	}
}
