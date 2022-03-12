// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (alex@gu3.me)

using Sandbox;

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
			buildWheel.AddOption( "Test 1", "tools/images/common/icon_info.png", () => Log.Trace( "1" ) );
			buildWheel.AddOption( "Test 2", "tools/images/common/icon_info.png", () => Log.Trace( "2" ) );
			buildWheel.AddOption( "Test 3", "tools/images/common/icon_info.png", () => Log.Trace( "3" ) );
			buildWheel.AddOption( "Test 4", "tools/images/common/icon_info.png", () => Log.Trace( "4" ) );
			buildWheel.AddOption( "Test 5", "tools/images/common/icon_info.png", () => Log.Trace( "5" ) );
		}

		if ( Input.Released( InputButton.Menu ) )
		{
			buildWheel?.Delete();
			buildWheel = null;
		}
	}
}
