using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

public class EnvironmentObjectSpot : AObject {

	public static List<EnvironmentObjectSpot> Instances = new List<EnvironmentObjectSpot>();

	[SerializeField]
	private GameObject[] _viewPrefabs;

	[SerializeField]
	private State _state;

	private GameObject _viewInstance;

	public enum State {

		Default,
		Empty,
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

		SetState( State.Destroyed );

		Debug.Log( this, this );

		EventSystem.RaiseEvent( new XenoTriggerEvent {Source = this} );
	}

	public void SetState( State state ) {

		_state = state;
		Debug.Log( state, this );

		switch ( state ) {

			case State.Infected:
				_viewInstance = Instantiate( _viewPrefabs.RandomElement() );
				_viewInstance.transform.SetParent( transform, worldPositionStays: false );
				break;

			case State.Destroyed:
				Destroy( _viewInstance );
				break;
		}
	}

	public State GetState() {

		return _state;
	}

}