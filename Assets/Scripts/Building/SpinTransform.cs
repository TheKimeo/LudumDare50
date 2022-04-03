using UnityEngine;

public class SpinTransform : MonoBehaviour
{
	[SerializeField] Transform m_TransformToSpin;
	[SerializeField] Vector3 m_Axis = Vector3.up;
	[SerializeField] float m_Speed = 50.0f;

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
		m_TransformToSpin.rotation *= Quaternion.AngleAxis( m_Speed * Time.deltaTime, m_Axis );
	}
}
