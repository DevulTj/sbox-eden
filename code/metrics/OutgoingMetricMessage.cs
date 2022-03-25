// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using System.Text.Json.Serialization;

namespace Eden;

/* This class is responsible for defining the schema for an outgoing message to the Metrics API. */
public class OutgoingMetricMessage
{
	[JsonPropertyName( "messageType" )]
	public string MessageType { get; set; } = "Metrics";

	[JsonPropertyName( "isServer" )]
	public bool IsServer { get; set; } = true;

	[JsonPropertyName( "authToken" )]
	public string AuthToken { get; set; }

	[JsonPropertyName( "playerId" )]
	public string PlayerId { get; set; }

	[JsonPropertyName( "metricType" )]
	public string MetricType { get; set; }
}
