using System;
using UnityEngine;
using System.Collections;
using UniRx;

public class GameplayController : MonoBehaviour {
	
	[SerializeField]
	private PlayerCharacterSpawner _playerSpawner;

	[SerializeField]
	private SpawnerBase[] _enemySpawners;

	[SerializeField]
	private ZoneSpawner[] _zoneSpawners;

    public static GameplayController Instance { get; private set; }

	void Awake() {
		Instance = this;
	}

	public IEnumerator Start() {
		yield return null;
		_playerSpawner.Initialize();
		foreach ( var each in _enemySpawners ) {
			each.Initialize();
		}

		foreach ( var each in _zoneSpawners ) {
			
			each.Initialize();
		}
	}

	[ContextMenu("Hook dependencies")]
	private void HookDependencies() {
		_playerSpawner = FindObjectOfType<PlayerCharacterSpawner>();
		_enemySpawners = FindObjectsOfType<SpawnerBase>();
		_zoneSpawners = FindObjectsOfType<ZoneSpawner>();
	}
}