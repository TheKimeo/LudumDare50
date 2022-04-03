using System.Collections.Generic;
using UnityEngine;

public class BuildingState : MonoBehaviour
{
	[SerializeField] Health m_Health;
	[SerializeField] float m_DisableOperationHealthRatio = 0.3f;
	[SerializeField] float m_ScaleOperationHealthRatio = 1.0f;

	HashSet<object> m_Suppressors = new HashSet<object>();

	//0.0f to 1.0f, No operation to fully operational
	public float OperationalRatio => m_Suppressors.Count == 0
		? CalculateOperationalRatio()
		: 0.0f;

	float CalculateOperationalRatio()
	{
		return Mathf.Clamp01( ( m_Health.HealthRatio - m_DisableOperationHealthRatio ) / ( m_ScaleOperationHealthRatio - m_DisableOperationHealthRatio ) );
	}

	public void AddSuppressor( object obj )
	{
		m_Suppressors.Add( obj );
	}

	public void RemoveSuppressor( object obj )
	{
		m_Suppressors.Remove( obj );
	}
}
