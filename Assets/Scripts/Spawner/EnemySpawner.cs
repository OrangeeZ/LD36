using System;
using System.Linq;
using UnityEngine;
using UniRx;

public class EnemySpawner : SpawnerBase {

	public Action<Character> Spawned;

	public EnemyCharacterInfo characterInfo;
	public EnemyCharacterStatusInfo characterStatusInfo;

	public ItemInfo[] startingItems;

	public float SpawnInterval;
	public float SpawnMoveSpeed;
	private float _startTime;

	private Character _character;

	[Expressions.CalculatorExpression]
	public StringReactiveProperty Activation;

	private Expressions.ReactiveCalculator _reactCalc;

	[Expressions.CalculatorExpression]
	public StringReactiveProperty Deactivation;

	private Expressions.ReactiveCalculator _reactCalcDeact;

	public override void Initialize() {

		_reactCalc = new Expressions.ReactiveCalculator( Activation );
		_reactCalcDeact = new Expressions.ReactiveCalculator( Deactivation );
		Spawn();

	}

	private void Spawn() {
		_startTime = 0.0f;

		if ( _reactCalc.Result.Value < 0 ) {
			return;
		}

		if ( _reactCalcDeact.Result.Value >= 0 ) {
			return;
		}

		_character = characterInfo.GetCharacter( startingPosition: transform.position, replacementStatusInfo: characterStatusInfo );

		foreach ( var each in startingItems.Select( _ => _.GetItem() ) ) {
			_character.Inventory.AddItem( each );
		}

		if ( characterStatusInfo != null ) {

			_character.ItemsToDrop = characterStatusInfo.ItemsToDrop;
			_character.dropProbability = characterStatusInfo.DropChance;

			var weapon = characterStatusInfo.Weapon1.GetItem();
			_character.Inventory.AddItem( weapon );

			weapon.Apply();
		}

	}

	private void Update() {
		_startTime += Time.deltaTime;

		if ( _startTime >= SpawnInterval ) {
			Spawn();
		}
	}

}