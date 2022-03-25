// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eden;

/* This class is responsible for handling the WebSocket messages to the Metrics API. */
public class MetricsWebSocketClient
{
	private readonly WebSocket ws;
	private readonly string connectionString;
	private readonly string authToken;

	public MetricsWebSocketClient( MetricsConfig configuration )
	{
		ws = new();
		connectionString = configuration.ConnectionString;
		authToken = configuration.AuthToken;

		ws.OnMessageReceived += OnMessageReceived;
	}

	public async Task<bool> Connect()
	{
		await ws.Connect( connectionString );
		return ws.IsConnected;
	}

	public bool Disconnect()
	{
		ws.Dispose();
		return !ws.IsConnected;
	}

	public async Task Send( OutgoingMetricMessage message )
	{
		Log.Info( "Sending message" );
		message.AuthToken = authToken;
		await ws.Send( JsonSerializer.Serialize( message ) );
	}

	private void OnMessageReceived( string message )
	{
		throw new NotImplementedException();
	}
}
