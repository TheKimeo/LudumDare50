using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
	static DontDestroyOnLoad m_Instance = null;

	private void Awake()
	{
		if ( m_Instance != null )
		{
			gameObject.SetActive( false );
			GameObject.Destroy( gameObject );
			return;
		}

		m_Instance = this;
		Object.DontDestroyOnLoad( this );
	}
}