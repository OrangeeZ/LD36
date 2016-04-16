using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

public class EnvironmentObject : AObject {

	public static List<EnvironmentObject> Instances = new List<EnvironmentObject>();

	public enum State {

		Default,
		Infected,
		Destroyed

	}

	private void Awake() {

		Instances.Add( this );
	}

	private void OnDestroy() {

		Instances.Remove( this );
	}

	public void Destroy() {

		Debug.Log( this, this );

		EventSystem.RaiseEvent( new XenoTriggerEvent {Source = this} );
	}

	public void SetState( State state ) {

		Debug.Log( state, this );
	}

}