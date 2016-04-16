using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.UI.Game_UI;
using Assets.UniRx.Scripts.Ui;
using Packages.EventSystem;
using UniRx;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : UIScreen
{

    private Character _character;

    [SerializeField]
    private HealthView _healthBar;
    [SerializeField]
    private WeaponView _weaponView;
    [SerializeField]
    private ScanerView _scanerController;

    [SerializeField]
    private Text _acornValue;

    [SerializeField]
    private Image _whiteImage;


    private void Awake()
    {

        EventSystem.Events.SubscribeOfType<PlayerCharacterSpawner.Spawned>(SetCharacter);
        EventSystem.Events.SubscribeOfType<BossDeadStateInfo.Dead>(OnBossDead);
    }

    private void OnBossDead(BossDeadStateInfo.Dead dead)
    {

        StartCoroutine(FadeAndWinScreen());
    }

    private IEnumerator FadeAndWinScreen()
    {

        var fadeDuration = 2f;

        var from = _whiteImage.color;
        var to = from;
        to.a = 1f;

        var timer = new AutoTimer(fadeDuration);

        while (timer.ValueNormalized < 1f)
        {

            _whiteImage.color = Color.Lerp(from, to, timer.ValueNormalized);

            yield return null;
        }
        //_whiteImage.CrossFadeAlpha( 0f, fadeDuration, ignoreTimeScale: true );

        //yield return new WaitForSeconds( fadeDuration );

        foreach (var each in Character.Instances)
        {
            each.Dispose();
        }

        SceneManager.LoadScene(2);
    }

    public void SetCharacter(PlayerCharacterSpawner.Spawned spawnedEvent)
    {
        _character = spawnedEvent.Character;
        _healthBar.Initialize(_character);
        _weaponView.Initialize(_character);
        _scanerController.Initialize(_character);
    }

    private void Update()
    {
        if (_character != null)
        {
        }
    }

}