using System;
using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : UIScreen {

	private Character _character;

	[SerializeField]
	private Slider _healthBar;

	[SerializeField]
	private Text _healthValue;

	[SerializeField]
	private Text _acornValue;

	[SerializeField]
	private Image _whiteImage;

	private void Awake() {

		EventSystem.Events.SubscribeOfType<PlayerCharacterSpawner.Spawned>( SetCharacter );
		EventSystem.Events.SubscribeOfType<BossDeadStateInfo.Dead>( OnBossDead );
	}

	private void OnBossDead( BossDeadStateInfo.Dead dead ) {

		StartCoroutine( FadeAndWinScreen() );
	}

	private IEnumerator FadeAndWinScreen() {

		var fadeDuration = 1f;
		_whiteImage.CrossFadeAlpha( 1f, fadeDuration, ignoreTimeScale: true );

		yield return new WaitForSeconds( fadeDuration );

		SceneManager.LoadScene( 2 );
	}

	public void SetCharacter( PlayerCharacterSpawner.Spawned spawnedEvent ) {

		_character = spawnedEvent.Character;
	}

	private void Update() {

		if ( _character != null ) {

			_healthBar.value = _character.Health.Value / _character.Status.MaxHealth.Value;
			_healthValue.text = _character.Health.Value.RoundToInt().ToString();

			var acornCount = _character.Inventory.GetItemCount<AcornAmmoItemInfo.AcornAmmo>();

			_acornValue.text = acornCount.ToString();
		}
	}

}