using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExternalEnemyController : MonoBehaviour {

	[SerializeField]
	private ExternalEnemyInfo _enemyInfo;

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

			var enemyInstance = Instantiate( _enemyPrefabs.RandomElement(), transform.position, Quaternion.identity ) as ExternalEnemy;

			enemyInstance.AttackCount = _enemyInfo.Type == ExternalEnemyType.Permanent ? int.MaxValue : 1;
			enemyInstance.AttackInterval = _enemyInfo.AttackCooldown;
			enemyInstance.Damage = _enemyInfo.Damage;
			enemyInstance.AttackTarget = _roomDevice;
            enemyInstance.Controller = this;

			enemyInstance.Initialize();

			_spawnedEnemies.Add( enemyInstance );
		}
	}

	public void RemoveEnemy( ExternalEnemy enemy ) {

		_spawnedEnemies.Remove( enemy );

		Destroy( enemy.gameObject );
	}
}
