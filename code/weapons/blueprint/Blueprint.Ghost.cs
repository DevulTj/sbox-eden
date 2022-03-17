// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.Component;

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
			RenderColor = Color.White.WithAlpha( 0.2f );

			var glow = Components.GetOrCreate<Glow>();
			glow.Active = true;
			glow.Color = Color.Cyan;
			glow.RangeMax = 1024;
			glow.RangeMin = 0;
		}

		/// <summary>
		/// Show snapped position to player
		/// </summary>
		[Event.PreRender]
		public void OnPreRender()
		{
			if ( SceneObject == null || Blueprint == null )
				return;

			var snapReference = Blueprint.FindBestSnapPoint();

			if ( snapReference == null )
				return;

			SceneObject.Transform = snapReference?.SnapPoint.Transform ?? default;
		}
	}
}
