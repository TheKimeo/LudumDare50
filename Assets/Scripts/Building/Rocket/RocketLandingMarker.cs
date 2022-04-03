using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class RocketLandingMarker : MonoBehaviour
{
	public static List<RocketLandingMarker> AllLandingMarkers { get; } = new List<RocketLandingMarker>();

	void OnEnable()
	{
		AllLandingMarkers.Add( this );
	}

	void OnDisable()
	{
		AllLandingMarkers.Remove( this );
	}
}
