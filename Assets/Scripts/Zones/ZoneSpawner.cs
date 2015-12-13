using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneSpawner : MonoBehaviour {

	[SerializeField]
	private ZoneInfo _zoneInfo;

	private List<Character> _characters = new List<Character>();

	private float _healingAmount;

	public void Initialize() {

		var view = Instantiate( _zoneInfo.ZonePrefab, transform.position, transform.rotation ) as ZoneView;

		view.CharacterEntered += OnCharacterEnter;
		view.CharacterExited += OnCharacterExit;

		_healingAmount = _zoneInfo.HealthPool;
	}

	private void Update() {

		if ( _healingAmount <= 0 ) return;

		foreach ( var each in _characters ) {

			each.Health.Value += _zoneInfo.HealingSpeed * Time.deltaTime;
		}

		_healingAmount -= _zoneInfo.HealingSpeed * Time.deltaTime;
	}

	private void OnCharacterEnter( Character character ) {

		_characters.Add( character );
	}

	private void OnCharacterExit( Character character ) {

		_characters.Remove( character );
	}

}