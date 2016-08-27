using UnityEngine;
using Assets.Scripts.UI.Game_UI;
using Assets.UniRx.Scripts.Ui;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

public class GameUI : UIScreen {

	private Character _character;

	[SerializeField]
	private HealthView _healthBar;

	[SerializeField]
	private WeaponView _weaponView;

	[SerializeField]
	private Text _acornValue;

    [SerializeField]
    private GameObject _win;

	[SerializeField]
	private Image _whiteImage;

	[SerializeField]
	private Text _alienCounter;

	private void Awake() {

		EventSystem.Events.SubscribeOfType<PlayerCharacterSpawner.Spawned>( SetCharacter );
		EventSystem.Events.SubscribeOfType<BossDeadStateInfo.Dead>( OnBossDead );
	}

	private void Start() {

	}

	private void OnBossDead( BossDeadStateInfo.Dead dead ) {

        _win.SetActive(true);

    }

	public void SetCharacter( PlayerCharacterSpawner.Spawned spawnedEvent ) {
		_character = spawnedEvent.Character;
		_healthBar.Initialize( _character );
		_weaponView.Initialize( _character );
	}
}