using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExternalEnemyController : MonoBehaviour {

	[SerializeField]
	private RoomDevice _roomDevice;
	
	[SerializeField]
	private ExternalEnemy[] _enemyPrefabs;

	private List<ExternalEnemy> _spawnedEnemies;

	private float _spawnInterval = 5f;

	IEnumerator Start() {

		_spawnedEnemies = new List<ExternalEnemy>();

		while ( true ) {
			
			yield return new WaitForSeconds( _spawnInterval );

			_roomDevice.Damage( 100f );

			_spawnedEnemies.Add( Instantiate( _enemyPrefabs.RandomElement() ) );
		}
	}
}
