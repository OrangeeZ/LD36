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
	private ScanerViewOld1 _scanerController;

	[SerializeField]
	private Text _acornValue;

    [SerializeField]
    private GameObject _win;

	[SerializeField]
	private Image _whiteImage;

	private AlienLossController _aliensLossController;

	[SerializeField]
	private Text _alienCounter;

	private void Awake() {

		EventSystem.Events.SubscribeOfType<PlayerCharacterSpawner.Spawned>( SetCharacter );
		EventSystem.Events.SubscribeOfType<BossDeadStateInfo.Dead>( OnBossDead );
	}

	private void Start() {

		_aliensLossController = FindObjectOfType<AlienLossController>();
	}

	private void OnBossDead( BossDeadStateInfo.Dead dead ) {

        _win.SetActive(true);

    }

	public void SetCharacter( PlayerCharacterSpawner.Spawned spawnedEvent ) {
		_character = spawnedEvent.Character;
		_healthBar.Initialize( _character );
		_weaponView.Initialize( _character );
		_scanerController.Initialize( _character );
	}

	private void Update() {
		if ( _character != null ) {
		}

		_alienCounter.text = _aliensLossController.AliensRemaining.ToString();
	}

}