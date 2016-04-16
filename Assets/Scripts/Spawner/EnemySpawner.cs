using System;
using System.Linq;
using UnityEngine;
using UniRx;

public class EnemySpawner : SpawnerBase {

	public Action<Character> Spawned;

	public EnemyCharacterInfo characterInfo;
	public EnemyCharacterStatusInfo characterStatusInfo;

	public float SpawnInterval;
	public int SpawnLimit;

	private float _startTime;
	private int _spawnCount;

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

		if ( SpawnLimit > 0 && _spawnCount >= SpawnLimit ) {
			return;
		}
		_spawnCount += 1;

		_character = characterInfo.GetCharacter( startingPosition: transform.position, replacementStatusInfo: characterStatusInfo );

		if ( characterStatusInfo != null ) {
			_character.ItemsToDrop = characterStatusInfo.ItemsToDrop;
			_character.dropProbability = characterStatusInfo.DropChance;
			_character.speakProbability = characterStatusInfo.SpeakChance;

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