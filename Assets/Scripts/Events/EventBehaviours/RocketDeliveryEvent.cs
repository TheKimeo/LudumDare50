using UnityEngine;

[CreateAssetMenu( fileName = "RocketDeliveryEvent", menuName = "Events/Rocket Delivery", order = 0 )]
public class RocketDeliveryEvent : EventBehaviour
{
	public override void ApplyEvent( EventManager.Event e )
	{
		RocketLandingManager rocketLandingManager = RocketLandingManager.Instance;
		
		bool success = rocketLandingManager.TryStartLanding();
		if ( success == false )
		{
			//TODO: Tell the player about this somehow?
			Debug.LogError( "Failed to start demo rocket landing" );
		}
	}

	public override float CalculateDuration( float difficulty )
	{
		RocketLandingManager rocketLandingManager = RocketLandingManager.Instance;
		return rocketLandingManager.RocketLandingDuration;
	}
}
