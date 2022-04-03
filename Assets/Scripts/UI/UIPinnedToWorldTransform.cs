using System.Collections.Generic;
using UnityEngine;

public class UIPinnedToWorldTransform : Singleton<UIPinnedToWorldTransform>
{
	[SerializeField] RectTransform m_PrefabsParent;

	static List<RectTransform> s_ToRemoveBuffer = new List<RectTransform>();

	Dictionary<RectTransform, Transform> m_PinnedUI = new Dictionary<RectTransform, Transform>();

	private void LateUpdate()
	{
		Camera camera = Camera.main;

		s_ToRemoveBuffer.Clear();
		foreach ( KeyValuePair<RectTransform, Transform> pair in m_PinnedUI )
		{
			RectTransform uiTransform = pair.Key;
			Transform targetTransform = pair.Value;

			if ( targetTransform == null )
			{
				s_ToRemoveBuffer.Add( uiTransform );
				continue;
			}

			Vector2 size = m_PrefabsParent.rect.size;
			Vector2 ViewportPosition = camera.WorldToViewportPoint( targetTransform.position );
			Vector2 WorldObject_ScreenPosition = new Vector2(
				( ViewportPosition.x * size.x ) - ( size.x * 0.5f ),
				( ViewportPosition.y * size.y ) - ( size.y * 0.5f ) );

			uiTransform.anchoredPosition = WorldObject_ScreenPosition;
		}

		foreach ( RectTransform toRemove in s_ToRemoveBuffer )
		{
			m_PinnedUI.Remove( toRemove );
			GameObject.Destroy( toRemove.gameObject );
		}
	}

	public RectTransform InstantiateAndPin( RectTransform prefab, Transform target )
	{
		RectTransform newPrefab = GameObject.Instantiate( prefab, m_PrefabsParent );
		m_PinnedUI.Add( newPrefab, target );
		return newPrefab;
	}

	public void DestroyAndRemovePin( RectTransform uiInstance )
	{
		bool didRemove = m_PinnedUI.Remove( uiInstance );
		if ( didRemove )
		{
			GameObject.Destroy( uiInstance.gameObject );
		}
	}
}
