using UnityEngine;

public class SpinTransform : MonoBehaviour
{
	[SerializeField] Transform m_TransformToSpin;
	[SerializeField] Vector3 m_Axis = Vector3.up;
	[SerializeField] float m_Speed = 50.0f;
	[SerializeField] BuildingState m_BuildingState;

	private void Start()
	{
		if ( m_TransformToSpin == null )
		{
			Debug.Assert( false, "[SpinTransform] No provided transform to spin for object: " + gameObject, gameObject );
			return;
		}
	}

	private void Update()
	{
		if ( m_BuildingState != null && m_BuildingState.OperationalRatio <= 0.0f )
		{
			return;
		}

		m_TransformToSpin.rotation *= Quaternion.AngleAxis( m_Speed * Time.deltaTime, m_Axis );
	}
}
