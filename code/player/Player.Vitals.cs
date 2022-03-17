// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class Player
{
	[Net]
	public IList<Vital> Vitals { get; set; }

	protected void SetupVitals()
	{
		Vitals.Add( new()
		{
			Name = "Hunger",
			DefaultValue = 250,
			MaxValue = 500,
		} );

		Vitals.Add( new()
		{
			Name = "Thirst",
			DefaultValue = 150,
			MaxValue = 300,
		} );

		Vitals.Add( new WetnessVital()
		{
			Name = "Wetness",
			DefaultValue = 0f,
			MaxValue = 100f,
			DrainSpeed = 300f
		} );
	}

	protected void ResetVitals()
	{
		Vitals.ToList()
			.ForEach( x => x.Reset() );
	}

	protected void TickVitals()
	{
		Vitals.ToList().ForEach( x => x.Tick( this ) );
	}

	public Vital GetVital( string name )
	{
		return Vitals.FirstOrDefault( x => x.Name == name );
	}
}
