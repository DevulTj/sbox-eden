// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Threading.Tasks;

namespace Eden;

[UseTemplate]
public partial class ResourceNotifications : Panel
{
	public static ResourceNotifications Current { get; protected set; }
	public ResourceNotifications()
	{
		Current = this;
	}

	[ClientRpc]
	public static void AddResource( int amount, string name )
	{
		bool positive = amount >= 0;

		var panel = Current.AddChild<Panel>( "notify" );
		panel.Add.Label( $"{( positive ? "+" : "-" )}{amount}", "amount" );
		panel.Add.Label( $"{name}", "name" );

		_ = Current.MarkForDelete( panel );
	}

	async Task MarkForDelete( Panel panel )
	{
		await Task.Delay( 4000 );
		panel.Delete();
	}
}
