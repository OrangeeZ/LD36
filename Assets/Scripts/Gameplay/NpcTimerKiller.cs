using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class NpcTimerKiller : MonoBehaviour {

	[SerializeField]
	private AudioClip _vilhelmScream;

	[SerializeField]
	private float _interval = 10f;

	[SerializeField]
	private PlayerCharacterSpawner _playerCharacterSpawner;

	private AutoTimer _timer;

	private void Start() {

		_timer = new AutoTimer( _interval );

		EventSystem.Events.SubscribeOfType<XenoDeadStateInfo.Dead>( OnXenoDie );
	}

	private void Update() {

		if ( _timer.ValueNormalized >= 1f ) {

			_timer.Reset();

			var characterRoom = Room.FindRoomForPosition( _playerCharacterSpawner.character.Pawn.position );
			var randomNpc = Room.GetRandomNpc( roomToSkip: characterRoom );
			if ( randomNpc != null ) {

				AudioSource.PlayClipAtPoint( _vilhelmScream, randomNpc.Pawn.position );

				randomNpc.Damage( 9999 );

				Debug.Log( "Kill NPC" );
			}
		}
	}

	private void OnXenoDie( XenoDeadStateInfo.Dead eventObject ) {

		Debug.Log( "Reset timer" );

		_timer.Reset();
	}

}