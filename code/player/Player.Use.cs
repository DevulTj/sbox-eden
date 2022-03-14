// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class Player
{
	/// <summary>
	/// This should be called somewhere in your player's tick to allow them to use entities
	/// </summary>
	protected override void TickPlayerUse()
	{
		// This is serverside only
		if ( !Host.IsServer ) return;

		// Turn prediction off
		using ( Prediction.Off() )
		{
			var usable = FindUsable();

			if ( Input.Pressed( InputButton.Use ) )
			{
				Using = usable;

				if ( Using == null )
				{
					UseFail();
					return;
				}
			}

			var weapon = ActiveChild as Weapon;
			if ( weapon.IsValid() )
				weapon.CanGrabSomething = usable.IsValid();

			if ( !Input.Down( InputButton.Use ) )
			{
				StopUsing();
				return;
			}

			if ( !Using.IsValid() )
				return;

			// If we move too far away or something we should probably ClearUse()?

			//
			// If use returns true then we can keep using it
			//
			if ( Using is IUse use && use.OnUse( this ) )
			{
				weapon?.OnUse( To.Single( Client ) );

				return;
			}

			StopUsing();
		}
	}

	/// <summary>
	/// Player tried to use something but there was nothing there.
	/// Tradition is to give a dissapointed boop.
	/// </summary>
	protected override void UseFail()
	{
		PlaySound( "player_use_fail" );

		var weapon = ActiveChild as Weapon;
		weapon?.OnUse( To.Single( Client ) );
	}

	/// <summary>
	/// If we're using an entity, stop using it
	/// </summary>
	protected override void StopUsing()
	{
		Using = null;
	}

	/// <summary>
	/// Find a usable entity for this player to use
	/// </summary>
	protected override Entity FindUsable()
	{
		// First try a direct 0 width line
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 85 )
			.Ignore( this )
			.Run();

		// See if any of the parent entities are usable if we ain't.
		var ent = tr.Entity;
		while ( ent.IsValid() && !IsValidUseEntity( ent ) )
		{
			ent = ent.Parent;
		}

		// Nothing found, try a wider search
		if ( !IsValidUseEntity( ent ) )
		{
			tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 85 )
			.Radius( 2 )
			.Ignore( this )
			.Run();

			// See if any of the parent entities are usable if we ain't.
			ent = tr.Entity;
			while ( ent.IsValid() && !IsValidUseEntity( ent ) )
			{
				ent = ent.Parent;
			}
		}

		// Still no good? Bail.
		if ( !IsValidUseEntity( ent ) ) return null;

		return ent;
	}
}
