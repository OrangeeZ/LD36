using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

public class EnvironmentObjectSpot : AObject {

	[SerializeField]
	private GameObject[] _viewPrefabs;

	[SerializeField]
	private State _state;

	[SerializeField]
	private bool _isInvincible = false;

	private GameObject _viewInstance;

	public enum State {

		Default,
		Empty,
		Infected,
		Destroyed

	}

	private void Start() {

		if ( _state == State.Default ) {

			_viewInstance = gameObject;
		}
	}

	public virtual void Destroy( Character hittingCharacter ) {

		if ( !_isInvincible ) {

			SetState( State.Destroyed );
		}

		Debug.Log( this, this );

		EventSystem.RaiseEvent( new XenoTriggerEvent {Source = this} );
	}

	public void TryResetState() {

		if ( _state != State.Destroyed ) {

			Destroy( _viewInstance );

			_state = State.Empty;
		}
	}

	public void SetState( State state ) {

		_state = state;
		Debug.Log( state, this );

		switch ( state ) {

			case State.Infected:
				_viewInstance = Instantiate( _viewPrefabs.RandomElement() );
				_viewInstance.transform.SetParent( transform, worldPositionStays: false );
				_viewInstance.transform.rotation = Quaternion.identity;
				//Destroy( _viewInstance.GetComponent<Collider>() );
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