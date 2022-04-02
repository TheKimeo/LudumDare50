using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	[SerializeField] Resource[] m_Resources;

	private void Awake()
	{
		for ( int i = 0; i < m_Resources.Length; ++i )
		{
			m_Resources[ i ].Initialise();
		}
	}
}
