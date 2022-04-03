using UnityEngine;

[CreateAssetMenu( fileName = "RocketDeliveryEvent", menuName = "Events/Rocket Delivery", order = 0 )]
public class RocketDeliveryEvent : EventBehaviour
{
	public override void ApplyEvent( EventManager.Event e )
	{
		RocketLandingManager rocketLandingManager = RocketLandingManager.Instance;
		rocketLandingManager.TryStartLanding();
	}

	public override float CalculateDuration( float difficulty )
	{
		RocketLandingManager rocketLandingManager = RocketLandingManager.Instance;
		return rocketLandingManager.RocketLandingDuration;
	}
}
