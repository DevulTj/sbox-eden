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

		[Event.Frame]
		public void OnFrameUpdate()
		{
			var snappedTransform = TraceForward( Owner, 128f ).EndPosition;
			Position = snappedTransform;
			Rotation = Rotation.Identity;
		}

		[Event.PreRender]
		public void OnPreRender()
		{
			var snappedTransform = Blueprint?.GetSnappedTransform( new Transform( TraceForward( Owner ).EndPosition, Rotation.Identity ) ) ?? default;
			SceneObject.Transform = snappedTransform;
		}

		// TODO: bit of a shit place to put this
		internal List<Transform> GetSnapPoints()
		{
			var list = new List<Transform>();

			var datas = Model?.GetData<ModelSnapPoints[]>();

			if ( datas == null )
				return new();

			var data = datas[0];
			var snapPoints = data.SnapPoints;
			var snapTransforms = snapPoints.Select( attachment => GetAttachment( attachment ) );
			snapTransforms.ToList().ForEach( snapPoint => list.Add( snapPoint ?? default ) );

			return list;
		}
	}
}
