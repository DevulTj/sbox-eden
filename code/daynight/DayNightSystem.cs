using Sandbox;

namespace Eden.DayNight;

public partial class DayNightSystem : Entity
{
	public static DayNightSystem Instance { get; protected set; }

	public delegate void SectionChanged( TimeSection section );
	public event SectionChanged OnSectionChanged;

	[Net, Predicted]
	public TimeSection Section { get; protected set; }

	[Net, Predicted]
	public float TimeOfDay { get; set; } = 9f;

	[ConVar.Replicated( "eden_daynight_speed", Help = "How fast the day night cylce is" )]
	public static float DayNightSpeed { get; set; } = 0.2f;

	public DayNightSystem()
	{
		Instance = this;
	}

	public override void Spawn()
	{
		base.Spawn();
		Transmit = TransmitType.Always;
	}

	public static TimeSection ToSection( float time )
	{
		if ( time > 5f && time <= 9f )
			return TimeSection.Dawn;
		if ( time > 9f && time <= 18f )
			return TimeSection.Day;
		if ( time > 18f && time <= 21f )
			return TimeSection.Dusk;

		return TimeSection.Night;
	}

	// Shared Tick
	[Event.Tick]
	protected void Tick()
	{
		TimeOfDay += DayNightSpeed * Time.Delta;

		if ( TimeOfDay >= 24f )
			TimeOfDay = 0f;

		var currentSection = ToSection( TimeOfDay );
		if ( currentSection != Section )
		{
			Section = currentSection;
			OnSectionChanged?.Invoke( currentSection );
		}
	}
}
