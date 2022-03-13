// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.Linq;

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

			var buildingTypes = Library.GetAttributes<DisplayOnBuildWheelAttribute>();
			buildingTypes.ToList().ForEach( t =>
				buildWheel.AddOption( t.Title, t.Icon, () => Log.Trace( $"Build {t.Title}" ) ) );
		}

		if ( Input.Released( InputButton.Menu ) )
		{
			buildWheel?.Delete();
			buildWheel = null;
		}
	}
}
