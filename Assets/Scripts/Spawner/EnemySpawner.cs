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

	private void Start() {

		_reactCalc = new Expressions.ReactiveCalculator( Activation );
		_reactCalc.SubscribeProperty( "dangerLevel", GameplayController.instance.dangerLevel );

		_reactCalcDeact = new Expressions.ReactiveCalculator( Deactivation );
		_reactCalcDeact.SubscribeProperty( "dangerLevel", GameplayController.instance.dangerLevel );

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
			_character.inventory.AddItem( each );
		}

		_character.itemToDrop = ItemToDrop;
		_character.dropProbability = DropProbability;

		if ( startingWeapon != null ) {

			var weapon = startingWeapon.GetItem();

			_character.inventory.AddItem( weapon );

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