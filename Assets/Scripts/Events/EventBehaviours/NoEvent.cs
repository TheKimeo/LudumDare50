using UnityEngine;

[CreateAssetMenu( fileName = "NoEvent", menuName = "Events/NoEvent", order = 0 )]
public class NoEvent : EventBehaviour
{
	public override void ApplyEvent( EventManager.Event e )
	{
		Debug.Log( "[NoEvent] Applied event" );
	}

	public override float CalculateDuration( float difficulty )
	{
		return 1.0f;
	}

	public override void OnQueued( EventManager.Event e )
	{
	}
}