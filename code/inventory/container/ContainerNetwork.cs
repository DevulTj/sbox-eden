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

		container.Move( slotA, slotB );
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

		// @TODO: unfuck this later
		container.Remove( slotA );

		var entity = WorldItemEntity.InstantiateFromPlayer( player, slot.Item );

		Log.Info( $"{entity} was spawned as a result of dropping an item" );
	}
}
