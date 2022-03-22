// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

[Library( "eden_container_box" )]
public partial class Box : WorldContainerEntity
{
	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/citizen_props/cardboardbox01.vmdl" );

		Container.SetSize( 10 );
		Container.Name = "Small Box";
	}
}
