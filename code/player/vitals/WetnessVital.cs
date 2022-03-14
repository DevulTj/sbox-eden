// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class WetnessVital : StatusVital
{
	public float AddSpeed => 10f;
	public override float TickSpeed => 0.5f;
	public override string ValueFormat => $"{Value:f0}%";

	protected override void OnVitalTick( Player player )
	{
		var waterLevel = player.WaterLevel;

		if ( waterLevel > 0 )
		{
			Value += waterLevel * AddSpeed;
			Value = Value.Clamp( 0, MaxValue );
		}
		else
		{
			Value -= DrainSpeed * Time.Delta;
			// Clamp
			Value = Value.Clamp( 0, MaxValue );
		}
	}
}
