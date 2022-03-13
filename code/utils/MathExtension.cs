﻿// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;

namespace Eden;

public static class MathExtension
{
	/// <summary>
	/// Calculate shortest difference between self and target.
	/// </summary>
	public static float DistanceAngle( this float self, float target )
	{
		float delta = ( target - self ).UnsignedMod( 360.0f );

		if ( delta > 180.0f )
		{
			delta -= 360.0f;
		}

		return delta;
	}

	/// <summary>
	/// Lerp towards an angle, taking the shortest difference into account.
	/// </summary>
	public static float LerpToAngle( this float self, float target, float delta )
	{
		float distance = self.DistanceAngle( target );

		if ( distance > -delta && delta > distance )
		{
			return target;
		}

		target = self + distance;

		return self.LerpTo( target, delta );
	}

	/// <summary>
	/// Does the opposite of what Atan2 does.
	/// <para>Finds the point (x, y) from the <paramref name="angle"/> given</para>
	/// </summary>
	/// <param name="angle">Degrees</param>
	/// <param name="length">How far away you want to be from the origin</param>
	public static Vector2 InverseAtan2( float angle, float length = 1f )
	{
		Vector2 res = new();

		float theta = angle * ( MathF.PI / 180f );

		res.x = length * MathF.Cos( theta );
		res.y = length * MathF.Sin( theta );

		return res;
	}
}
