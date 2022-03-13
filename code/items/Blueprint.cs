// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Linq;

namespace Eden;

[Library( "eden_blueprint", Title = "Blueprint", Spawnable = false )]
partial class Blueprint : BaseCarriable
{
	RadialWheel buildWheel;

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( !IsClient )
			return;

		if ( Input.Pressed( InputButton.Menu ) )
			CreateBuildWheel();

		if ( Input.Released( InputButton.Menu ) )
			DeleteBuildWheel();
	}

	private void CreateBuildWheel()
	{
		if ( buildWheel != null )
			return;

		buildWheel = RadialWheel.Create();

		var buildingTypes = Library.GetAttributes<DisplayOnBuildWheelAttribute>();
		buildingTypes.ToList().ForEach( t =>
			buildWheel.AddOption( t.Title, t.Icon, () => Log.Trace( $"Build {t.Title}" ) ) );
	}

	private void DeleteBuildWheel()
	{
		buildWheel?.Delete();
		buildWheel = null;
	}
}
