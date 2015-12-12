using System;
using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UnityEngine.UI;

public class RestartScreen : UIScreen {

    [SerializeField]
    private Button _restartButton;

    private void Start() {

        _restartButton.onClick.AddListener( OnRestartClick );
    }

    private void OnRestartClick() {

        foreach ( var each in Character.instances ) {
            
            each.Dispose();
        }

        EventSystem.Reset();

        GameplayController.instance.dangerLevel.Value = 0;

        Character.instances.Clear();

        Application.LoadLevel( Application.loadedLevel );
    }

}