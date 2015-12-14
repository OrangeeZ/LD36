using System;
using System.Linq;
using UnityEngine;
using UniRx;

public class OneEnemySpawner : SpawnerBase {

	public Action<Character> Spawned;
	public EnemyCharacterInfo characterInfo;
	public EnemyCharacterStatusInfo characterStatusInfo;
	public ItemInfo[] startingItems;

	//public float Interval = 20f;
	private float _lastSpawnTime = 0f;
	private bool _isActive = false;

	private Character _character;

	public override void Initialize() {

		Spawn();

		_isActive = true;
	}

	private void Spawn() {

		_character = characterInfo.GetCharacter( startingPosition: transform.position, replacementStatusInfo: characterStatusInfo );

		foreach ( var each in startingItems.Select( _ => _.GetItem() ) ) {

			_character.Inventory.AddItem( each );
		}

		if ( characterStatusInfo != null ) {

			_character.ItemsToDrop = characterStatusInfo.ItemsToDrop;

			_character.dropProbability = characterStatusInfo.DropChance;
			_character.speakProbability = characterStatusInfo.SpeakChance;

			var weapon = characterStatusInfo.Weapon1.GetItem();
			_character.Inventory.AddItem( weapon );

			weapon.Apply();
		}
	}

	private void OnValidate() {

		name = string.Format( "One Enemy Spawner [{0}]", characterStatusInfo == null ? "null" : characterStatusInfo.name );
	}

	private void Update() {

		if ( !_isActive ) {

			return;
		}

		if ( characterStatusInfo.SpawnInterval >= 0 && ( Time.timeSinceLevelLoad - _lastSpawnTime ) >= characterStatusInfo.SpawnInterval ) {

			Spawn();

			_lastSpawnTime = Time.timeSinceLevelLoad;
		}
	}

}