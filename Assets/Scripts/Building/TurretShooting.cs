using System.Collections.Generic;
using UnityEngine;

public class TurretShooting : MonoBehaviour
{
	public AudioSource m_turretAudio;

	[SerializeField] Transform m_CenterPoint; //This is so the rotations are relative to the center of the barrels
	[SerializeField] Transform m_HorizontalRotator;
	[SerializeField] Transform m_VerticalRotator;
	[SerializeField] BuildingState m_BuildingState;
	[Space]
	[SerializeField] float m_HorizontalSpeed;
	[SerializeField] float m_VerticalSpeed;
	[Space]
	[SerializeField] float m_MaxTargetRange;
	[SerializeField] float m_DelayBetweenShots;
	[SerializeField] float m_ShotDamage;

	Transform m_Target;
	float m_ShootDelay;

	void Start()
	{
		m_Target = null;
		m_ShootDelay = m_DelayBetweenShots;
	}

	void Update()
	{
		if ( m_BuildingState != null && m_BuildingState.OperationalRatio <= 0.0f )
		{
			return;
		}

		if ( m_Target != null && ValidTarget( m_Target ) == false )
		{
			//Clear target if current target is invalid
			m_Target = null;
		}

		if ( m_Target == null )
		{
			TryFindTarget();
			m_ShootDelay = m_DelayBetweenShots;
		}
		else
		{
			m_ShootDelay -= Time.deltaTime;
		}

		if ( m_Target != null )
		{
			AimAtTarget();
		}

		if ( m_ShootDelay <= 0.0f )
		{
			ShootAtTarget();
			m_ShootDelay = m_DelayBetweenShots;
		}
	}

	void ShootAtTarget()
	{
		Debug.Assert( m_Target != null );

		Health health = m_Target.GetComponent<Health>();
		if ( health == null )
		{
			Debug.Assert( false, "[TurretShooting] Cannot shoot at target " + m_Target + " as it has no health component", m_Target );
			return;
		}

		health.Modify( -m_ShotDamage );
	}

	void TryFindTarget()
	{
		Vector3 sourcePosition = m_CenterPoint.position;

		List<TurretTargetable> allTargets = TurretTargetable.AllTargets;

		Transform closestTarget = null;
		float closestSqrDistance = float.MaxValue;
		foreach ( TurretTargetable target in allTargets )
		{
			Transform targetTransform = target.transform;
			if ( ValidTarget( targetTransform ) == false )
			{
				continue;
			}

			float sqrDistance = ( sourcePosition - targetTransform.position ).sqrMagnitude;
			if ( sqrDistance < closestSqrDistance )
			{
				closestTarget = targetTransform;
				closestSqrDistance = sqrDistance;
			}
		}

		m_Target = closestTarget;

		if(m_Target != null && !m_turretAudio.isPlaying)
        {
			m_turretAudio.Play();
        }
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
		if ( target.position.y <= m_CenterPoint.position.y )
		{
			//Target shouldn't be below the center of the turret
			return false;
		}

		float distanceSqr = ( target.position - m_CenterPoint.position ).sqrMagnitude;
		if ( distanceSqr > ( m_MaxTargetRange * m_MaxTargetRange ) )
		{
			//Out of range
			return false;
		}

		return true;
	}
}
