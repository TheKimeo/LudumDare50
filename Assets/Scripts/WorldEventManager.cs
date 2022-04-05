using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
	private int m_layerMask;

	private WorldButton m_Hovered;
	private WorldButton m_Down;

	private void Awake()
	{
		m_layerMask = LayerMask.GetMask( "WorldButton" );
		m_Hovered = null;
		m_Down = null;
	}

	private void Update()
	{
		UpdateTarget();

		if ( Input.GetKeyUp( KeyCode.Mouse0 ) && m_Down != null )
		{
			m_Down.m_OnMouseClick.Invoke();
		}
		else if ( Input.GetKeyDown( KeyCode.Mouse0 ) && m_Hovered != null )
		{
			m_Down = m_Hovered;
		}
	}

	void UpdateTarget()
	{
		Camera camera = Camera.main;
		Transform cameraTransform = camera.transform;

		Vector3 point = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		bool hit = Physics.Raycast( point, cameraTransform.forward, out RaycastHit hitInfo, float.MaxValue, m_layerMask );
		if ( hit == false )
		{
			m_Hovered?.m_OnMouseUnHover.Invoke();
			m_Hovered = null;
			m_Down = null;
			return;
		}

		WorldButton toHover = hitInfo.collider.GetComponent<WorldButton>();

		if ( toHover != m_Hovered )
		{
			m_Hovered?.m_OnMouseUnHover.Invoke();
			m_Hovered = toHover;
			m_Hovered.m_OnMouseHover.Invoke();
		}
	
	}
}
