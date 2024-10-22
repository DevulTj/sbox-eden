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
		var player = ConsoleSystem.Caller.Pawn as Player;
		var guid = Guid.Parse( guidString );
		var container = Get( guid );

		if ( container is null )
			return;

		if ( !container.HasAccess( player ) )
			return;

		Log.Info( $"container: {guidString}, slotA: {slotA}, slotB: {slotB}" );

		container.Move( slotA, slotB );

		UpdatePlayer( To.Single( player.Client ), guidString );
	}

	[ServerCmd( "eden_container_move_external" )]
	public static void ContainerMoveExternal( string guidString, int slotA, string destinationGuidString, int slotB )
	{
		var player = ConsoleSystem.Caller.Pawn as Player;
		var container = Get( Guid.Parse( guidString ) );
		var destinationContainer = Get( Guid.Parse( destinationGuidString ) );

		if ( container is null || destinationContainer is null )
			return;

		if ( !container.HasAccess( player ) || !destinationContainer.HasAccess( player ) )
			return;

		Log.Info( $"container: {guidString}, slotA: {slotA}, slotB: {slotB}" );

		container.Move( slotA, slotB, destinationContainer );

		UpdatePlayer( To.Single( player.Client ), guidString );
		UpdatePlayer( To.Single( player.Client ), destinationGuidString );
	}

	[ServerCmd( "eden_container_itemaction" )]
	public static void DoItemAction( string guidString, int slotA, string id )
	{
		var player = ConsoleSystem.Caller.Pawn as Player;
		var guid = Guid.Parse( guidString );
		var container = Get( guid );

		if ( container is null )
			return;

		if ( !container.HasAccess( player ) )
			return;

		var slot = container.GetSlot( slotA );
		if ( slot is null || slot.Item is null )
			return;

		int count = slot.Item.DoAction( player, id, slot );
		if ( count > 0 )
		{
			slot.SetQuantity( slot.Quantity - count );

			if ( slot.Quantity < 1 )
			{
				container.Remove( slotA );
				UpdatePlayer( To.Single( player.Client ), guidString );
			}
		}
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

			ItemEntity.Create( player, item, quantity );
		}
	}
}
