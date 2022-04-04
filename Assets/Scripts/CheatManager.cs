using UnityEngine;

public class CheatManager : MonoBehaviour
{
	[SerializeField] Resource[] m_Resources;
	[SerializeField] Population m_Population;

#if UNITY_EDITOR
	void Update()
	{
		for (int i = 0; i < m_Resources.Length; ++i )
		{
			if ( Input.GetKeyDown( KeyCode.Alpha1 + i ) )
			{
				Resource resource = m_Resources[ i ];
				resource.ModifyCap( 5000 );
				resource.Modify( 5000 );
			}
		}

		if ( Input.GetKeyDown( KeyCode.Alpha1 + m_Resources.Length ) )
		{
			m_Population.ModifyCap( 5000 );
			m_Population.Modify( 5000 );
		}
	}
#endif
}
