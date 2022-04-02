using UnityEngine;

[CreateAssetMenu( fileName = "DisasterEvent", menuName = "Events/DisasterEvent", order = 0 )]
public class DisasterEvent : EventBehaviour
{
	public override void ApplyEvent( EventManager.Event e )
	{
		DisasterManager disasterManager = DisasterManager.Instance;
		disasterManager.TriggerDisaster();
	}
}
