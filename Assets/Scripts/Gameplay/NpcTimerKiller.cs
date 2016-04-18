using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class NpcTimerKiller : MonoBehaviour {

	[SerializeField]
	private float _interval = 10f;

	private AutoTimer _timer;

	void Start() {
		
		_timer = new AutoTimer( _interval );

		EventSystem.Events.SubscribeOfType<XenoDeadStateInfo.Dead>( OnXenoDie );
	}

	void Update() {

		if ( _timer.ValueNormalized >= 1f ) {
			
			_timer.Reset();

			var randomNpc = Room.GetRandomNpc();
			if ( randomNpc != null ) {

				Room.GetRandomNpc().Damage( 9999 );

				Debug.Log( "Kill NPC" );
			}
		}
	}

	private void OnXenoDie( XenoDeadStateInfo.Dead eventObject ) {
		
		Debug.Log( "Reset timer" );

		_timer.Reset();
	}

}
