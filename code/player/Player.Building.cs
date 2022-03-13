// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;

namespace Eden;

partial class Player
{
	RadialWheel buildWheel;

	private void TickBuilding()
	{
		if ( !IsClient )
			return;

		if ( Input.Pressed( InputButton.Menu ) && buildWheel == null )
		{
			buildWheel = RadialWheel.Create();

			List<string> buildingTypes = new( new[] {
				"door",
				"floor",
				"roof",
				"stairs",
				"wall",
				"window"
			} );

			buildingTypes.ForEach( t =>
			{
				buildWheel.AddOption( t.ToTitleCase(), $"ui/building/{t}.png", () => Log.Trace( $"Build {t}" ) );
			} );
		}

		if ( Input.Released( InputButton.Menu ) )
		{
			buildWheel?.Delete();
			buildWheel = null;
		}
	}
}
