using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneSpawner : MonoBehaviour {

	[SerializeField]
	private ZoneInfo _zoneInfo;

	private List<Character> _characters = new List<Character>();

	private float _healingAmount;
	private ZoneView _view;

	public void Initialize() {

		_view = Instantiate( _zoneInfo.ZonePrefab, transform.position, transform.rotation ) as ZoneView;

		_view.transform.localScale = _zoneInfo.Size;
		_view.CharacterEntered += OnCharacterEnter;
		_view.CharacterExited += OnCharacterExit;

		_healingAmount = _zoneInfo.HealthPool;
	}

	private void Update() {

		if ( _healingAmount <= 0 ) {

			return;
		}

		foreach ( var each in _characters ) {

			var healingAmount = each.Status.ModifierCalculator.CalculateFinalValue( ModifierType.SunHealthRestore, _zoneInfo.HealingSpeed );
			var healingPerDeltaTime = healingAmount * Time.deltaTime;

			each.Health.Value += healingPerDeltaTime;
			_healingAmount -= healingPerDeltaTime;
		}

		var rate = _healingAmount / _zoneInfo.HealthPool;
		_view.UpdateIntensity( rate );
	}

	private void OnCharacterEnter( Character character ) {

		_characters.Add( character );
	}

	private void OnCharacterExit( Character character ) {

		_characters.Remove( character );
	}

}