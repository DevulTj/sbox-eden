// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Text.Json.Serialization;

namespace Eden;

/* This class is responsible for reading config from the s&box data folder for the WS connection to the Metrics API. */
public static class ConfigReader
{
	private static string CONFIG_FILENAME = "config.json";

	public static MetricsConfig GetMetricsConfig()
	{
		if ( !FileSystem.Data.FileExists( CONFIG_FILENAME ) )
		{
			return null;
		}

		MetricsConfig metricsConfig = FileSystem.Data.ReadJson<MetricsConfig>( CONFIG_FILENAME );
		return metricsConfig;
	}

}

/* This class is responsible for defining a schema for the config file. */
public class MetricsConfig
{
	[JsonPropertyName( "connectionString" )]
	public string ConnectionString { get; set; }

	[JsonPropertyName( "authToken" )]
	public string AuthToken { get; set; }

}
