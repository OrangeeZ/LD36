using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class HealbotObject : EnvironmentObjectSpot {

	[SerializeField]
	private float _cooldown = 5f;

	[SerializeField]
	private GameObject _activeState;

	[SerializeField]
	private GameObject _inactiveState;

	private float _cooldownTime;

	private void Start() {

		EventSystem.Events.SubscribeOfType<Room.EveryoneDied>( OnEveryoneDieInRoom );
	}

	private void OnEveryoneDieInRoom( Room.EveryoneDied everyoneDiedEvent ) {

		if ( everyoneDiedEvent.Room.GetRoomType() != Room.RoomType.MedicalBay ) {

			return;
		}

		enabled = false;

		_activeState.SetActive( false );
		_inactiveState.SetActive( true );
	}

	public override void Destroy( Character hittingCharacter ) {

		if ( Time.time < _cooldownTime || !enabled ) {

			return;
		}

		hittingCharacter.Heal( 999 );

		_cooldownTime = Time.time + _cooldown;
	}

	private void Update() {

		var isReady = Time.time > _cooldownTime;

		_activeState.SetActive( isReady );
		_inactiveState.SetActive( !isReady );
	}

}