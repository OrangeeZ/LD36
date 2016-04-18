using UnityEngine;
using System.Collections;

public class HealbotObject : EnvironmentObjectSpot {

	[SerializeField]
	private float _cooldown = 5f;

	[SerializeField]
	private GameObject _activeState;

	[SerializeField]
	private GameObject _inactiveState;

	private float _cooldownTime;

	public override void Destroy( Character hittingCharacter ) {

		if ( Time.time < _cooldownTime ) return;

		hittingCharacter.Heal( 999 );

		_cooldownTime = Time.time + _cooldown;
	}

	private void Update() {

		var isReady = Time.time > _cooldownTime;

		_activeState.SetActive( isReady );
		_inactiveState.SetActive( !isReady );
	}

}