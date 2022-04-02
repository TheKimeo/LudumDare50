using UnityEngine;

public class Singleton<T> : MonoBehaviour
	where T : Singleton<T>
{
	public static T Instance { get; private set; } = null;

	protected virtual void Awake()
	{
		if ( Instance != null )
		{
			GameObject.Destroy( gameObject );
			Debug.Assert( false );
		}

		Instance = (T) this;
	}
}
