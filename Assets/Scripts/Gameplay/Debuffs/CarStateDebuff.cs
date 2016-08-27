using UnityEngine;
using System.Collections;

public class CarStateDebuff : ScriptableObject {

	public float IntactPenalty;
	public float BrokenPenalty;

	public virtual void ApplyTo( CarStateController carStateController ) {
		
	}
}
