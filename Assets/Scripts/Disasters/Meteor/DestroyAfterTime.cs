using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	[SerializeField] float m_Config_Delay;

	float m_Delay;

    void Start()
    {
		m_Delay = m_Config_Delay;
	}

    void Update()
    {
		m_Delay -= Time.deltaTime;

		if (m_Delay > 0.0f)
		{
			return;
		}

		GameObject.Destroy( gameObject );
    }
}
