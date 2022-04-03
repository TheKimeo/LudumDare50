using UnityEngine;

public class ShakeTransform : MonoBehaviour
{
	[SerializeField] Transform m_TransformToSpin;
	[SerializeField] Vector3 m_Axis = Vector3.up;
	[SerializeField] float m_Speed = 2.0f;
	[SerializeField] float m_Magnitude = 0.5f;
	[SerializeField] BuildingState m_BuildingState;

	Vector3 m_StartingLocalPosition;

	private void Start()
	{
		if ( m_TransformToSpin == null )
		{
			Debug.Assert( false, "[SpinTransform] No provided transform to spin for object: " + gameObject, gameObject );
			return;
		}

		m_StartingLocalPosition = m_TransformToSpin.localPosition;
	}

	private void Update()
	{
		if ( m_BuildingState != null && m_BuildingState.OperationalRatio <= 0.0f )
		{
			return;
		}

		float offset = ( Time.time * m_Speed ) % Mathf.PI;
		m_TransformToSpin.localPosition = m_StartingLocalPosition + m_Axis * Mathf.Sin( offset ) * m_Magnitude;
	}
}
