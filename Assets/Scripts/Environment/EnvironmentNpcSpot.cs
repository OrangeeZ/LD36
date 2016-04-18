using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class EnvironmentNpcSpot : EnvironmentObjectSpot {

	[SerializeField]
	private EnemySpawner _spawner;

	private Character _controlledCharacter;

	void Start() {

		EventSystem.Events.SubscribeOfType<Character.Died>( OnCharacterDie );
	}

	private void OnCharacterDie( Character.Died dieEvent ) {

		if ( dieEvent.Character == _controlledCharacter ) {
			
			Destroy( null );
		}
	}

	protected override void SetInfectedState() {

		_spawner.Initialize();
		_controlledCharacter = _spawner.GetLastSpawnedCharacter();
	}

}
