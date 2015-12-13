using System;
using UnityEngine;
using System.Collections;
using UniRx;

public class GameplayController : MonoBehaviour {
	
	[SerializeField]
	private PlayerCharacterSpawner _playerSpawner;

	[SerializeField]
	private EnemySpawner[] _enemySpawners;

    public static GameplayController Instance { get; private set; }

	void Awake() {

		Instance = this;
	}

	public IEnumerator Start() {

		yield return null;

		_playerSpawner.Initialize();

		//ScreenManager.GetWindow<GameUI>().SetCharacter( _playerSpawner );

		foreach ( var each in _enemySpawners ) {
			
			each.Initialize();
		}
	}
}