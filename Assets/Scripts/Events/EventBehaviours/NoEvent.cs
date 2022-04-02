using UnityEngine;

[CreateAssetMenu( fileName = "NoEvent", menuName = "Events/NoEvent", order = 0 )]
public class NoEvent : EventBehaviour
{
	public override void ApplyEvent( EventManager.Event e )
	{
		Debug.Log( "[NoEvent] Applied event" );
	}
}