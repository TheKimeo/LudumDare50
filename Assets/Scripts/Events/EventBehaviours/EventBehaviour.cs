using UnityEngine;

public abstract class EventBehaviour : ScriptableObject
{
	abstract public void ApplyEvent( EventManager.Event e );
}
