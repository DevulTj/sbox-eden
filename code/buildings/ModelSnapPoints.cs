// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
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
	[JsonPropertyName( "snap_points" ), FGDType( "model_attachment" )]
	public string[] SnapPoints { get; set; }
}
