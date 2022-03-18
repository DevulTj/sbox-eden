// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eden;

public partial class ResourceManager
{
	/// <summary>
	/// Container for a list of entities we are tracking
	/// </summary>
	protected List<Entity> TrackedEntities { get; set; } = new();
	protected List<ResourceEntity> Resources { get; set; } = new();

	/// <summary>
	/// How often the resource manager refreshes
	/// </summary>
	protected virtual int RefreshTimeSeconds => 5;
	/// <summary>
	/// The range away from each tracked entity.
	/// </summary>
	protected virtual float RefreshRange => 512f;

	/// <summary>
	/// Check decay every 60 seconds, as it's expensive
	/// </summary>
	protected virtual float CheckDecayTime => 60f;
	/// <summary>
	/// How long until an individual resource will decay from being isolated.
	/// </summary>
	protected virtual float IndividualDecayTime => 120f;
	protected TimeSince CheckedDecayingEntities { get; set; } = 0;

	private bool Initialized { get; set; } = false;
	private Task CurrentTask { get; set; }

	public void Initialize()
	{
		if ( Initialized ) return;

		Initialized = true;
		CurrentTask = Refresh();
	}

	protected void RefreshEntity( Entity entity )
	{
		var list = Entity.FindInSphere( entity.Position, RefreshRange )
			.OfType<ResourceEntity>()
			.ToList();

		list.ForEach( x =>
		{
			x.LastRefresh = 0;
		} );
	}

	protected void CreateResource( Vector3 point )
	{
		//
	}

	protected void CheckDecay()
	{
		CheckedDecayingEntities = 0;

		for ( int i = Resources.Count - 1; i >= 0; i-- )
		{
			ResourceEntity entity = Resources[i];

			if ( entity.LastRefresh > IndividualDecayTime )
			{
				entity.Delete();
				Resources.RemoveAt( i );
			}
		}
	}

	protected async Task Refresh()
	{
		TrackedEntities.ForEach( entity => RefreshEntity( entity ) );

		if ( CheckedDecayingEntities >= CheckDecayTime )
			CheckDecay();

		await GameTask.DelaySeconds( RefreshTimeSeconds );

		CurrentTask = Refresh();
	}
}
