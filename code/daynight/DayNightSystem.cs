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

	public float Speed { get; set; } = 0.2f;

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
		TimeOfDay += Speed * Time.Delta;

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
