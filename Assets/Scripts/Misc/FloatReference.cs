using UnityEngine;

[CreateAssetMenu( fileName = "Float_", menuName = "Type References/Float", order = 0 )]
public class FloatReference : ScriptableObject
{
	[SerializeField] float m_Value;

	public float Value => m_Value;
}
