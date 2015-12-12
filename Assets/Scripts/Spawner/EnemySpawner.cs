using System;
using System.Linq;
using UnityEngine;
using UniRx;

public class EnemySpawner : AObject {

	public Action<Character> Spawned;

	public CharacterInfo characterInfo;

	public ItemInfo[] startingItems;

	public WeaponInfo startingWeapon;

	public ItemInfo ItemToDrop;
	public float DropProbability = 0.15f;

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

	public void Initialize() {

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

		_character = characterInfo.GetCharacter( startingPosition: transform.position );

		foreach ( var each in startingItems.Select( _ => _.GetItem() ) ) {
			_character.Inventory.AddItem( each );
		}

		_character.itemToDrop = ItemToDrop;
		_character.dropProbability = DropProbability;

		if ( startingWeapon != null ) {

			var weapon = startingWeapon.GetItem();

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