using System;
using System.Linq;
using UnityEngine;
using UniRx;

public class OneEnemySpawner : SpawnerBase {

	public Action<Character> Spawned;
	public EnemyCharacterInfo characterInfo;
	public EnemyCharacterStatusInfo characterStatusInfo;
	public ItemInfo[] startingItems;
	public float DropProbability = 0.15f;


	private Character _character;


	public override void Initialize() {
		Spawn();
	}


	private void Spawn() {
		_character = characterInfo.GetCharacter( startingPosition: transform.position, replacementStatusInfo: characterStatusInfo );

		foreach ( var each in startingItems.Select( _ => _.GetItem() ) ) {
			_character.Inventory.AddItem( each );
		}

		_character.ItemsToDrop = characterStatusInfo.ItemsToDrop;
		_character.dropProbability = DropProbability;

		var weapon = characterStatusInfo.Weapon1.GetItem();
		_character.Inventory.AddItem( weapon );

		weapon.Apply();
	}
}