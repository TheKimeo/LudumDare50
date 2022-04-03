using UnityEngine;

[RequireComponent( typeof( SphereCollider ) )]
[ExecuteAlways]
public class BuildingCollisionSphere : MonoBehaviour
{
	[SerializeField] FloatReference m_SphereRadius;

	void Start()
	{
		if ( m_SphereRadius == null )
		{
			Debug.LogError( "[BuildingCollisionSphere] Must provide a float reference for sphere radius for object: " + gameObject, gameObject );
			return;
		}

		GetComponent<SphereCollider>().radius = m_SphereRadius.Value;
	}

#if UNITY_EDITOR
	void Update()
	{
		if ( m_SphereRadius != null )
		{
			GetComponent<SphereCollider>().radius = m_SphereRadius.Value;
		}
	}
#endif
}
