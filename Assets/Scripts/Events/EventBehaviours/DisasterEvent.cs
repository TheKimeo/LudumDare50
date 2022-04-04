using UnityEngine;

[CreateAssetMenu( fileName = "DisasterEvent", menuName = "Events/DisasterEvent", order = 0 )]
public class DisasterEvent : EventBehaviour
{
	public override void ApplyEvent( EventManager.Event e )
	{
		DisasterManager disasterManager = DisasterManager.Instance;
		disasterManager.TriggerDisaster( e.m_Duration );
	}

	public override float CalculateDuration( float difficulty )
	{
		DisasterManager disasterManager = DisasterManager.Instance;
		return disasterManager.CalculateDisasterDuration( difficulty );
	}

	public override void OnQueued( EventManager.Event e )
	{
	}
}
