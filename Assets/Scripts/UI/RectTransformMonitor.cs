using UnityEngine;

[ExecuteAlways]
public class RectTransformMonitor : MonoBehaviour
{
	[SerializeField] RectTransform m_Transform;

	float m_A = 0.0f;

    // Update is called once per frame
    void Update()
    {
		m_A += m_Transform.sizeDelta.x;
    }
}
