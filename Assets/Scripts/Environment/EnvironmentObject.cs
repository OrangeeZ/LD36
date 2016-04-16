using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnvironmentObject : AObject {

	public static List<EnvironmentObject> Instances = new List<EnvironmentObject>();

	public enum State {

		Default,
		Infected,
		Destroyed

	}

	void Awake() {
		
		Instances.Add( this );
	}

	void OnDestroy() {

		Instances.Remove( this );
	}

	public void Destroy() {
		
		Debug.Log( this, this );
	}

	public void SetState( State state ) {
		
		Debug.Log( state, this );
	}

}