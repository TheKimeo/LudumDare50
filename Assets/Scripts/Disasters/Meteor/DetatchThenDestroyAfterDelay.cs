using UnityEngine;

public class DetatchThenDestroyAfterDelay : MonoBehaviour
{
	[SerializeField] float m_DelayToDestroy = 1.0f;

	float m_Delay = float.NaN;

	public void Detatch()
	{
		transform.SetParent( null, true );
		m_Delay = m_DelayToDestroy;
	}

	private void Update()
	{
		if ( float.IsNaN( m_Delay ) )
		{
			return;
		}

		m_Delay -= Time.deltaTime;

		if ( m_Delay <= 0.0f )
		{
			GameObject.Destroy( gameObject );
		}
	}
}
