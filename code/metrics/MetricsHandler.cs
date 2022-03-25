// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

/* This class is responsible for reading configuration and managing the Metrics WS Client. */
public class MetricsHandler
{
	private static MetricsWebSocketClient metricsWebSocketClient;

	public MetricsHandler()
	{
		var configuration = ConfigReader.GetMetricsConfig();
		InitializeMetricsConnection( configuration );

		// Sample Metric Send
		SendMetricForPlayer( "1234", "noobsOwned" );
	}

	private async void InitializeMetricsConnection( MetricsConfig configuration )
	{
		metricsWebSocketClient = new MetricsWebSocketClient( configuration );
		await metricsWebSocketClient.Connect();
	}

	public async void SendMetricForPlayer( string playerId, string metricType )
	{
		if ( !metricsWebSocketClient.IsConnected() ) return;

		OutgoingMetricMessage message = new();
		message.PlayerId = playerId;
		message.MetricType = metricType;

		await metricsWebSocketClient.Send( message );
	}
}
