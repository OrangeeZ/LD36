using UnityEngine;
using System.Collections;
using System.Linq;

public class AlienWinController : MonoBehaviour {

	[SerializeField]
	private EnemySpawner _xenoMotherSpawner;

	[SerializeField]
	private EnemySpawner _blackAlienSpawnerPrefab;

	[SerializeField]
	private PlayerCharacterSpawner _playerCharacterSpawner;

	// Update is called once per frame
	void Update () {

		if ( _playerCharacterSpawner.character == null ) {

			return;
		}

		var randomNpc = Room.GetRandomNpc( null );
		if ( randomNpc == null ) {
			
			Debug.Log( "You lose!" );

			_xenoMotherSpawner.Initialize();

			var playerRoom = Room.FindRoomForPosition( _playerCharacterSpawner.character.Pawn.position );
			var rooms = Room.GetRooms().Where( _ => _ != playerRoom );
			foreach ( var each in rooms ) {

				var spawnerInstance = Instantiate( _blackAlienSpawnerPrefab );
				spawnerInstance.position = each.transform.position;
				spawnerInstance.Initialize();
			}

			enabled = false;
		}
	}
}
