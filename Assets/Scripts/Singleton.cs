using UnityEngine;

public class Singleton<T> : MonoBehaviour
	where T : Singleton<T>
{
	public static T Instance { get; private set; } = null;

	protected virtual void Awake()
	{
		if ( Instance != null )
		{
			Debug.Assert( false, "Singleton: " + gameObject + " is a duplicate of singleton " + Instance + ". Destroying duplicate..." );
			GameObject.Destroy( gameObject );
		}

		Instance = (T) this;
	}
}
