// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;

namespace Eden;

public partial class GameHud : HudEntity<RootPanel>
{
	public static GameHud Current { get; set; }

	public GameHud()
	{
		if ( !IsClient )
			return;

		Current = this;

		RootPanel.StyleSheet.Load( "/ui/hud/GameHud.scss" );

		// List of elements used by the game
		RootPanel.AddChild<PlayerVitals>();
		RootPanel.AddChild<MainMenuPanel>();
		RootPanel.AddChild<HudHotbarPanel>();
		RootPanel.AddChild<WipText>();
		RootPanel.AddChild<EyeEntityVital>();
		RootPanel.AddChild<ResourceNotifications>();
		RootPanel.AddChild<DevMenu>();

		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<KillFeed>();
		RootPanel.AddChild<Crosshair>();
		RootPanel.AddChild<NameTags>();
	}
}
