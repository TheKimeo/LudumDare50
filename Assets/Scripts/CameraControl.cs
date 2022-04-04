using UnityEngine;

public class CameraControl : MonoBehaviour
{
	[SerializeField] Camera m_Camera;
	[SerializeField] AnimationCurve m_OrthographicSpeedMultiplier;
	[SerializeField] AnimationCurve m_MoveSpeedByZoomMultiplier;

	[SerializeField] float m_moveSpeed = 2;
	[SerializeField] float m_zoomSpeed = 10;
	[SerializeField] float m_inertiaBase = 1000;
	[SerializeField] float m_inertiaLerpTime = 4f;

	[SerializeField] float m_mapRadius = 70;

	[SerializeField] float m_minOrthographicSize = 1.0f;
	[SerializeField] float m_maxOrthographicSize = 10.0f;

	private float m_orthographicSize = 10.0f;
	private float m_intertiaTimeStart;
	private float m_scrollInertiaT = 0.0f;
	private float m_scrollInertia = 1000;
	private float m_inertiaDir = 1.0f;

	private int m_layerMask;

	void ResetInertia( float i_dir )
	{
		m_scrollInertiaT = 1.0f;
		m_scrollInertia = m_inertiaBase;
		m_inertiaDir = i_dir;
		m_intertiaTimeStart = Time.time;
	}


	public float GetZoomRatio()
    {
		return 1 - Mathf.Lerp(0, 1, m_orthographicSize / m_maxOrthographicSize);
	}

	private void Start()
	{
		float middlingSize = ( m_minOrthographicSize + m_maxOrthographicSize ) / 2.0f;
		SetOrthographicSize( middlingSize );
		m_layerMask = LayerMask.GetMask( "PlaceableGround" );
	}

	void Update()
	{
		float zoomRatio = ToOrthographicRatio( m_orthographicSize );

		//Move cam
		float horizontalMove = Input.GetAxis( "Horizontal" );
		float verticalMove = Input.GetAxis( "Vertical" );
		Vector2 inputDirectionRaw = new Vector2( horizontalMove, verticalMove );

		//Only normalize if outside of range
		Vector2 inputDirection = inputDirectionRaw.sqrMagnitude > 1.0f
			? inputDirectionRaw.normalized
			: inputDirectionRaw;

		Vector3 forward = ToXZPlane( transform.forward );
		Vector3 right = Vector3.Cross( Vector3.up, forward );

		float zoomMovementSpeedMultiplier = m_MoveSpeedByZoomMultiplier.Evaluate( zoomRatio );
		Vector3 move = forward * inputDirection.y + right * inputDirection.x;
		move *= m_moveSpeed * zoomMovementSpeedMultiplier * Time.deltaTime;

		Vector3 newPos = transform.position + move;

		//Clamp pos
		Vector2 focalPos2d = GetFocalPosition(newPos);
		float distance = Vector2.Distance( focalPos2d, Vector2.zero );
		if ( distance > m_mapRadius )
		{
			Vector2 inBoundsVec2 = focalPos2d * ( m_mapRadius / distance );
			Vector2 translation = inBoundsVec2 - focalPos2d;
			newPos.x += translation.x;
			newPos.z += translation.y;
		}

		//Zoom cam
		float scrollValue = -Input.GetAxis( "Mouse ScrollWheel" );
		if ( scrollValue != 0.0f )
		{
			m_orthographicSize += m_zoomSpeed * scrollValue * Time.deltaTime;
			ResetInertia( Mathf.Sign( scrollValue ) );
		}
		else if ( m_scrollInertiaT > 0.0f )
		{
			m_orthographicSize += m_scrollInertia * Time.deltaTime * m_inertiaDir;
			m_scrollInertia = Mathf.Lerp( m_inertiaBase, 0.0f, ( Time.time - m_intertiaTimeStart ) / m_inertiaLerpTime );
		}

		transform.position = newPos;

		float newSize = Mathf.Clamp( m_orthographicSize, m_minOrthographicSize, m_maxOrthographicSize );
		SetOrthographicSize( newSize );
	}

	Vector2 GetFocalPosition( Vector3 camPos )
	{
		bool succcess = Physics.Raycast( camPos, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, m_layerMask );
		if ( succcess )
		{
			return new Vector2( hit.point.x, hit.point.z );
		}

		Debug.Assert( false, "No ground?" );
		return Vector2.zero;
	}

	float ToOrthographicRatio( float orthographicSize )
	{
		return ( orthographicSize - m_minOrthographicSize ) / ( m_maxOrthographicSize - m_minOrthographicSize );
	}

	float FromOrthographicRatio( float orthographicRatio )
	{
		return orthographicRatio * ( m_maxOrthographicSize - m_minOrthographicSize ) + m_minOrthographicSize;
	}

	void SetOrthographicSize( float orthographicSize )
	{
		m_orthographicSize = orthographicSize;
		float ratio = ToOrthographicRatio( orthographicSize );
		float mappedRatio = m_OrthographicSpeedMultiplier.Evaluate( ratio );
		m_Camera.orthographicSize = FromOrthographicRatio( mappedRatio );
	}

	Vector3 ToXZPlane( Vector3 direction )
	{
		Vector3 xzDirection = new Vector3( direction.x, 0.0f, direction.z );
		return xzDirection.normalized;
	}
}
