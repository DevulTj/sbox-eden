// without permission of its author (gustaf@thrivingventures.com).

using Sandbox;

namespace Eden;

public partial class GameEvent
{
	public partial class Server
	{
	}

	public partial class Client
	{
		public const string BackpackChanged = "Client.Inventory.BackpackChanged";
		public const string HotbarChanged = "Client.Inventory.HotbarChanged";

		public class BackpackChangedAttribute : EventAttribute
		{
			public BackpackChangedAttribute() : base( BackpackChanged ) { }
		}

		public class HotbarChangedAttribute : EventAttribute
		{
			public HotbarChangedAttribute() : base( HotbarChanged ) { }
		}
	}

	public partial class Shared
	{
	}
}
