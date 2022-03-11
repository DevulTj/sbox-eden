// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

/// <summary>
/// Drives modification for a weapon's <see cref="ViewModel"/>
/// </summary>
public struct ViewModelData
{
	/// <summary>
	/// Think of this as how weighty a weapon is. The higher the value, the weightier it'll feel.
	/// </summary>
	public float SwingInfluence = 0.01f;

	/// <summary>
	/// How fast the weapon returns to its default position after being swung around.
	/// </summary>
	public float ReturnSpeed = 5.0f;

	/// <summary>
	/// A dampener on how far the swing influence can do its influencing.
	/// </summary>
	public float MaxOffsetLength = 10.0f;

	/// <summary>
	/// How fast a bob cycle is.
	/// </summary>
	public float BobCycleTime = 8;

	/// <summary>
	/// The direction of viewbob.
	/// </summary>
	public Vector3 BobDirection = new Vector3( 0.0f, -0.2f, 0.2f );

	/// <summary>
	/// Translates the ViewModel a set amount, always.
	/// </summary>
	public Vector3 Offset = new Vector3( 0.0f, 0.1f, 0.0f );
}
