using System.Collections.Generic;
using UnityEngine;

public class RocketLandingManager : Singleton<RocketLandingManager>
{
	struct Landing
	{
		public RocketLandingMarker m_Target;
		public Transform m_Rocket;
		public float m_StartTime;
		public bool m_HasLanded;
	}

	[SerializeField] Population m_population;
	[SerializeField] int m_populationAdd = 50;


	[SerializeField] GameObject m_RocketPrefab;
	[SerializeField] float m_RocketStartHeight;
	[Space]
	[SerializeField] float m_LandingDuration = 1.0f;
	[SerializeField] AnimationCurve m_LandingCurve;
	[Space]
	[SerializeField] float m_GroundedDuration = 1.0f;
	[Space]
	[SerializeField] float m_TakeOffDuration = 1.0f;
	[SerializeField] AnimationCurve m_TakeOffCurve;

	static List<int> s_IndexBuffer = new List<int>();
	List<Landing> m_ActiveLandings = new List<Landing>();

	public float RocketLandingDuration => m_LandingDuration + m_GroundedDuration + m_TakeOffDuration;

	void Update()
	{
		UpdateRocketHeights();
	}

	void UpdateRocketHeights()
	{
		TimeManager timeManager = TimeManager.Instance;
		float currentTime = timeManager.m_CurrentTime;

		for ( int i = m_ActiveLandings.Count - 1; i >= 0; --i )
		{
			Landing landing = m_ActiveLandings[ i ];

			if ( landing.m_Target == null || landing.m_Target.isActiveAndEnabled == false )
			{
				//Veto the landing as the target vanished/is too damaged
				//TODO: Explode?
				Debug.Log( "Interrupting rocket landing as target is no longer a valid landing point" );
				GameObject.Destroy( landing.m_Rocket.gameObject );
				m_ActiveLandings.RemoveAt( i );
			}

			float duration = currentTime - landing.m_StartTime;
			Debug.Assert( duration >= 0.0f );

			if ( duration <= m_LandingDuration )
			{
				float ratio = Mathf.Clamp01( duration / m_LandingDuration );
				float mappedRatio = m_LandingCurve.Evaluate( ratio );
				landing.m_Rocket.position = CalculateRocketPosition( landing.m_Target, mappedRatio );
				continue;
			}
			duration -= m_LandingDuration;

			if ( duration <= m_GroundedDuration )
			{
				if ( landing.m_HasLanded == false )
				{
					OnRocketLanded( landing );
					landing.m_HasLanded = true;
					m_ActiveLandings[ i ] = landing;
				}

				landing.m_Rocket.position = CalculateRocketPosition( landing.m_Target, 0.0f );
				continue;
			}
			duration -= m_GroundedDuration;

			if ( duration <= m_TakeOffDuration )
			{
				float ratio = Mathf.Clamp01( duration / m_LandingDuration );
				float mappedRatio = m_TakeOffCurve.Evaluate( ratio );
				landing.m_Rocket.position = CalculateRocketPosition( landing.m_Target, mappedRatio );
				continue;
			}
			duration -= m_TakeOffDuration;

			//Full animation complete, can delete our record of it
			GameObject.Destroy( landing.m_Rocket.gameObject );
			m_ActiveLandings.RemoveAt( i );
		}
	}

	//Height ratio is 0 for grounded, 1 for at max height
	Vector3 CalculateRocketPosition( RocketLandingMarker groundMarker, float heightRatio )
	{
		Vector3 groundPosition = groundMarker.transform.position;
		Vector3 rocketPosition = groundPosition;
		rocketPosition.y = ( m_RocketStartHeight - groundPosition.y ) * heightRatio + groundPosition.y;
		return rocketPosition;
	}

	//Can fail if no landing pads are available
	public bool TryStartLanding()
	{
		RocketLandingMarker marker = GetRandomLandingMarker();
		if (marker == null)
		{
			//No valid landing targets
			return false;
		}

		TimeManager timeManager = TimeManager.Instance;

		Vector3 rocketPosition = marker.transform.position;
		rocketPosition.y = m_RocketStartHeight;

		GameObject rocket = GameObject.Instantiate( m_RocketPrefab, rocketPosition, Quaternion.identity );
		Debug.Assert( rocket != null );

		m_ActiveLandings.Add( new Landing
		{
			m_Target = marker,
			m_Rocket = rocket.transform,
			m_StartTime = timeManager.m_CurrentTime,
			m_HasLanded = false,
		} );

		return true;
	}

	RocketLandingMarker GetRandomLandingMarker()
	{
		List<RocketLandingMarker> markers = RocketLandingMarker.AllLandingMarkers;

		List<int> indexBuffer = s_IndexBuffer;
		indexBuffer.SetupIndexBuffer( markers.Count );
		indexBuffer.Shuffle();

		foreach ( int index in indexBuffer )
		{
			RocketLandingMarker marker = markers[ index ];

			if ( IsMarkerReserved( marker ) )
			{
				continue;
			}

			return marker;
		}

		return null;
	}

	bool IsMarkerReserved( RocketLandingMarker marker )
	{
		foreach ( Landing landing in m_ActiveLandings )
		{
			if (landing.m_Target == marker )
			{
				return true;
			}
		}

		return false;
	}

	void OnRocketLanded( Landing landing )
	{
		m_population.Modify(m_populationAdd);
	}
}
