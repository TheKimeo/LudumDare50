using System.Collections.Generic;
using UnityEngine;

public class TurretTargetable : MonoBehaviour
{
	public static List<TurretTargetable> AllTargets { get; } = new List<TurretTargetable>();

	private void OnEnable()
	{
		AllTargets.Add( this );
	}

	private void OnDisable()
	{
		AllTargets.Remove( this );
	}
}
