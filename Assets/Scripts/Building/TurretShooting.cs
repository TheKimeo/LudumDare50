using System.Collections.Generic;
using UnityEngine;

public class TurretShooting : MonoBehaviour
{
	[SerializeField] Transform m_CenterPoint; //This is so the rotations are relative to the center of the barrels
	[SerializeField] Transform m_HorizontalRotator;
	[SerializeField] Transform m_VerticalRotator;
	[Space]
	[SerializeField] float m_HorizontalSpeed;
	[SerializeField] float m_VerticalSpeed;

	Transform m_Target;

	void Start()
	{
		m_Target = null;
	}

	void Update()
	{
		if ( m_Target != null && ValidTarget( m_Target ) == false )
		{
			//Clear target if current target is invalid
			m_Target = null;
		}

		if ( m_Target == null )
		{
			TryFindTarget();
		}

		if ( m_Target != null )
		{
			AimAtTarget();
		}
	}

	void TryFindTarget()
	{
		Vector3 sourcePosition = m_CenterPoint.position;

		List<TurretTargetable> allTargets = TurretTargetable.AllTargets;

		Transform closestTarget = null;
		float closestSqrDistance = float.MaxValue;
		foreach ( TurretTargetable target in allTargets)
		{
			Transform targetTransform = target.transform;
			if ( ValidTarget( targetTransform ) == false )
			{
				continue;
			}

			float sqrDistance = ( sourcePosition - targetTransform.position ).sqrMagnitude;
			if ( sqrDistance  < closestSqrDistance )
			{
				closestTarget = targetTransform;
				closestSqrDistance = sqrDistance;
			}
		}

		m_Target = closestTarget;
	}

	void AimAtTarget()
	{
		Vector3 direction = ( m_Target.position - m_CenterPoint.position ).normalized;
		Vector3 xzDirection = new Vector3( direction.x, 0.0f, direction.z ).normalized;

		Quaternion idealHorizontalRotation = Quaternion.LookRotation( -xzDirection, Vector3.up );
		m_HorizontalRotator.rotation = Quaternion.RotateTowards( m_HorizontalRotator.rotation, idealHorizontalRotation, m_HorizontalSpeed * Time.deltaTime );

		float verticalAngle = Vector3.Angle( xzDirection, direction );
		Quaternion idealVerticalRotation = Quaternion.AngleAxis( verticalAngle, Vector3.right );
		m_VerticalRotator.localRotation = Quaternion.RotateTowards( m_VerticalRotator.localRotation, idealVerticalRotation, m_VerticalSpeed * Time.deltaTime );
	}

	bool ValidTarget( Transform target )
	{
		//Target shouldn't be below the center of the turret
		return target.position.y > m_CenterPoint.position.y;
	}
}
