// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class GameEvent
{
	public partial class Client
	{
		public const string MainMenuOpened = "Client.MainMenu.MainMenuOpened";
		public const string MainMenuClosed = "Client.MainMenu.MainMenuClosed";

		public class MainMenuOpenedAttribute : EventAttribute
		{
			public MainMenuOpenedAttribute() : base( MainMenuOpened ) { }
		}

		public class MainMenuClosedAttribute : EventAttribute
		{
			public MainMenuClosedAttribute() : base( MainMenuClosed ) { }
		}
	}
}
