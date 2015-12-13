using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

public class GameUI : UIScreen {

	private Character _character;

	[SerializeField]
	private Slider _healthBar;

	private void Awake() {

		EventSystem.Events.SubscribeOfType<PlayerCharacterSpawner.Spawned>( SetCharacter );
	}

	public void SetCharacter( PlayerCharacterSpawner.Spawned spawnedEvent ) {

		_character = spawnedEvent.Character;
	}

	private void Update() {

		if ( _character != null ) {

			_healthBar.value = (float) _character.Health.Value / _character.Status.MaxHealth.Value;
		}
	}

}