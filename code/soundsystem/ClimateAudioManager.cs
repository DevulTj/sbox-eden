// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Threading.Tasks;

namespace Eden;

public partial class ClimateAudioManager : Entity
{
	public Sound CurrentSound { get; set; }

	public ClimateAudioManager()
	{
	}

	public override void Spawn()
	{
		base.Spawn();

		Transmit = TransmitType.Always;
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();

		_ = Start();
	}

	protected async Task Start()
	{
		// @TODO: this is a s&box workaround.
		await GameTask.DelaySeconds( 5 );

		CurrentSound = PlaySound( "eden.map.ambient.waves" );
	}
}
