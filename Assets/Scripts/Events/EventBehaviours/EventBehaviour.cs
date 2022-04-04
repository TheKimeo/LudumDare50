using UnityEngine;

public abstract class EventBehaviour : ScriptableObject
{
	[SerializeField] public bool m_VisibleOnTimeline = true;
	[SerializeField] public Sprite m_EventIcon;

	abstract public void OnQueued( EventManager.Event e );
	abstract public void ApplyEvent( EventManager.Event e );
	abstract public float CalculateDuration( float difficulty );
}
