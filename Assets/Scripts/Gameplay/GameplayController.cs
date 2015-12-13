using System;
using UnityEngine;
using System.Collections;
using UniRx;

public class GameplayController : MonoBehaviour {
	
	[SerializeField]
	private PlayerCharacterSpawner _playerSpawner;

	[SerializeField]
	private SpawnerBase[] _enemySpawners;

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
	}

	[ContextMenu("Hook dependencies")]
	private void HookDependencies() {
		_playerSpawner = FindObjectOfType<PlayerCharacterSpawner>();


		var sp = FindObjectsOfType<EnemySpawner>();
		var oneSp = FindObjectsOfType<OneEnemySpawner>();

		int length = 0;
		length += (sp != null) ? sp.Length : 0;
		length += (oneSp != null) ? oneSp.Length : 0;

		_enemySpawners = new SpawnerBase[length];
		int i = 0;
		if (sp != null) {
			sp.CopyTo (_enemySpawners, 0);
			i += sp.Length;
		}
		if (oneSp != null) {
			oneSp.CopyTo (_enemySpawners, i);
			i += oneSp.Length;
		}
	}
}