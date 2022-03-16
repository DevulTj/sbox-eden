
using Sandbox;
using Sandbox.Hooks;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Linq;

namespace Eden;

[Library( "EdenChatBox" )]
public partial class ChatBox : Panel
{
	static ChatBox Current;

	public Panel Canvas { get; protected set; }
	public TextEntry Input { get; protected set; }

	public ChatBox()
	{
		Current = this;

		StyleSheet.Load( "/ui/hud/chatbox/chatbox.scss" );

		Canvas = Add.Panel( "chat_canvas" );

		Input = Add.TextEntry( "" );
		Input.AddEventListener( "onsubmit", () => Submit() );
		Input.AddEventListener( "onblur", () => Close() );
		Input.AcceptsFocus = true;
		Input.AllowEmojiReplace = true;

		Chat.OnOpenChat += Open;
	}

	void Open()
	{
		AddClass( "open" );
		Input.Focus();
	}

	void Close()
	{
		RemoveClass( "open" );
		Input.Blur();
	}

	void Submit()
	{
		Close();

		var msg = Input.Text.Trim();
		Input.Text = "";

		if ( string.IsNullOrWhiteSpace( msg ) )
			return;

		Say( msg );
	}


	public void AddEntry( string name, string message, string avatar, string hexColor = "#ff0000" )
	{
		var e = Canvas.AddChild<ChatEntry>();
		//e.SetFirstSibling();
		e.Message.Text = message;
		e.NameLabel.Text = name;

		e.TimestampLabel.Text = e.Time.ToString( "h:mm tt" );

		e.NameLabel.Style.FontColor = Color.Parse( hexColor );

		e.Avatar.SetTexture( avatar );

		e.SetClass( "noname", string.IsNullOrEmpty( name ) );
		e.SetClass( "noavatar", string.IsNullOrEmpty( avatar ) );
	}


	[ClientCmd( "chat_add", CanBeCalledFromServer = true )]
	public static void AddChatEntry( string name, string message, string avatar = null, string hexColor = "#ff0000" )
	{
		Current?.AddEntry( name, message, avatar, hexColor );

		// Only log clientside if we're not the listen server host
		if ( !Global.IsListenServer )
		{
			Log.Info( $"{name}: {message}" );
		}
	}

	[ClientCmd( "chat_addinfo", CanBeCalledFromServer = true )]
	public static void AddInformation( string message, string avatar = null )
	{
		Current?.AddEntry( null, message, avatar );
	}


	public static Client Caller = null;
	[ServerCmd( "say" )]
	public static void Say( string message )
	{
		Assert.NotNull( ConsoleSystem.Caller );

		// todo - reject more stuff
		if ( message.Contains( '\n' ) || message.Contains( '\r' ) )
			return;

		var player = ConsoleSystem.Caller.Pawn as Player;

		Log.Info( $"{ConsoleSystem.Caller}: {message}" );

		AddChatEntry( To.Everyone, ConsoleSystem.Caller.Name, message, $"avatar:{ConsoleSystem.Caller.PlayerId}", "#98C1D9" );
	}

}
public static partial class Chat
{
	public static event Action OnOpenChat;

	[ClientCmd( "openchat" )]
	internal static void MessageMode()
	{
		OnOpenChat?.Invoke();
	}
}
