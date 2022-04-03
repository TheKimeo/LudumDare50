using UnityEngine;

public abstract class EventBehaviour : ScriptableObject
{
	[SerializeField] public Sprite m_EventIcon;

	abstract public void ApplyEvent( EventManager.Event e );
	abstract public float CalculateDuration( float difficulty );
}
