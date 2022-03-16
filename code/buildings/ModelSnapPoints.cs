// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Eden;

/// <summary>
/// Determines which attachments are used for snapping.
/// 
/// <para>
///	Snapped buildings will be rotated in the forward (X) direction for each attachment.
/// </para>
/// </summary>
[ModelDoc.GameData( "eden_snap_points", AllowMultiple = true )]
public class ModelSnapPoints
{
	public struct SnapPointData
	{
		[FGDType( "model_attachment" )]
		public string Attachment { get; set; }
	}

	[JsonPropertyName( "snap_points" )]
	public SnapPointData[] SnapPoints { get; set; }

	public static List<Transform> GetLocalSnapPointTransforms( Model model )
	{
		if ( model == null )
			Log.Error( "Model is null" );

		var snapPointNodes = model.GetData<ModelSnapPoints[]>();

		if ( snapPointNodes == null )
		{
			Log.Error( "Model has no snap point nodes" );
			return new();
		}

		var list = new List<Transform>();

		var snapPointNode = snapPointNodes[0];
		var snapPoints = snapPointNode.SnapPoints;
		var snapTransforms = snapPoints.Select( s => model.GetAttachment( s.Attachment ) );
		snapTransforms.ToList().ForEach( s => list.Add( s ?? default ) );

		return list;
	}
}
