// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

partial class Blueprint : Weapon
{
	private partial class GhostEntity : ModelEntity
	{
		internal Blueprint Blueprint { get; set; }

		public override void Spawn()
		{
			base.Spawn();
		}

		public void UpdateFromAsset( BuildingAsset asset )
		{
			Model = asset.BuildingModel;
			SetMaterialOverride( "materials/buildings/building_ghost.vmat" );
		}

		/// <summary>
		/// Update entity position to wherever the player is looking
		/// </summary>
		[Event.Frame]
		public void OnFrameUpdate()
		{
			var tracePosition = TraceForward( Owner ).EndPosition;
			Position = tracePosition;
			Rotation = Rotation.Identity;
		}

		/// <summary>
		/// Show snapped position to player
		/// </summary>
		[Event.PreRender]
		public void OnPreRender()
		{
			if ( SceneObject == null || Blueprint == null )
				return;

			var snappedTransform = Blueprint.GetSnappedTransform( new Transform( TraceForward( Owner ).EndPosition ) );
			SceneObject.Transform = snappedTransform;
		}

		public List<Transform> GetSnapPoints( bool worldSpace = true )
		{
			var snapPoints = ModelSnapPoints.GetSnapPoints( Model );

			if ( worldSpace )
				return snapPoints.Select( x => Transform.ToWorld( x ) ).ToList();

			return snapPoints;
		}
	}
}
