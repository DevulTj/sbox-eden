// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public struct ViewModelData
{
	public float SwingInfluence = 0.01f;
	public float ReturnSpeed = 5.0f;
	public float MaxOffsetLength = 10.0f;
	public float BobCycleTime = 8;
	public Vector3 BobDirection = new Vector3( 0.0f, -0.2f, 0.2f );
}
