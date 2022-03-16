// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class Vital : BaseNetworkable
{
	[Net]
	public string Name { get; set; } = "Vital";
	[Net]
	public float Value { get; set; } = 100f;
	[Net]
	public float MaxValue { get; set; } = 100f;

	// Server only variables for vitals
	public float DrainSpeed { get; set; } = 10f;

	// Tick interval (seconds)
	public virtual float TickSpeed => 1f;

	public TimeSince LastTick = 0;

	public virtual string ValueFormat => $"{Value:f0}";

	public void Reset()
	{
		Value = MaxValue;
	}

	protected virtual void OnVitalTick( Player player )
	{
		Value -= DrainSpeed * Time.Delta;
		// Clamp
		Value = Value.Clamp( 0, MaxValue );
	}

	public void Tick( Player player )
	{
		if ( LastTick > TickSpeed )
		{
			OnVitalTick( player );
			LastTick = 0;
		}
	}
}
