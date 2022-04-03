using UnityEngine;

public class Singleton<T> : MonoBehaviour
	where T : Singleton<T>
{
	public static T Instance
	{
		get
		{
			if ( m_Instance == null )
			{
				//Lazy fetch
				m_Instance = GameObject.FindObjectOfType<T>();
			}

			return m_Instance;
		}
	}

	private static T m_Instance = null;

	protected virtual void Awake()
	{
		if ( m_Instance == this )
		{
			return;
		}

		if ( m_Instance != null )
		{
			Debug.Assert( false, "Singleton: " + gameObject + " is a duplicate of singleton " + m_Instance + ". Destroying duplicate..." );
			GameObject.Destroy( gameObject );
		}

		m_Instance = (T) this;
	}
}
