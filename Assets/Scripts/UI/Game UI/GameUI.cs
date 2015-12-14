using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

public class GameUI : UIScreen {

	private Character _character;

	[SerializeField]
	private Slider _healthBar;

	[SerializeField]
	private Text _healthValue;

	[SerializeField]
	private Text _acornValue;

	private void Awake() {

		EventSystem.Events.SubscribeOfType<PlayerCharacterSpawner.Spawned>( SetCharacter );
	}

	public void SetCharacter( PlayerCharacterSpawner.Spawned spawnedEvent ) {

		_character = spawnedEvent.Character;
	}

	private void Update() {

		if ( _character != null ) {

			_healthBar.value = _character.Health.Value / _character.Status.MaxHealth.Value;
			_healthValue.text = _character.Health.Value.ToString();
			_acornValue.text = _character.Inventory.GetItemCount<AcornAmmoItemInfo.AcornAmmo>().ToString();
		}
	}

}