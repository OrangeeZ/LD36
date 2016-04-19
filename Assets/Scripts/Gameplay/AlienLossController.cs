using UnityEngine;
using System.Collections.Generic;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

public class AlienLossController : MonoBehaviour {

	[SerializeField]
	private EnemySpawner _motherSpawner;

    private List<Character> _enemies = new List<Character>();

    public static int AlienCount = 0;
	public int AliensRemaining { get; set; }

	// Use this for initialization
	private void Awake() {

		EventSystem.Events.SubscribeOfType<EnemySpawner.Spawned>( OnEnemySpawn );
		EventSystem.Events.SubscribeOfType<XenoDeadStateInfo.Dead>( OnXenoDie );
	}

	private void OnXenoDie( XenoDeadStateInfo.Dead eventObject ) {

		AlienCount--;

		AliensRemaining = AlienCount;
		
		Debug.LogFormat( "Alien count: {0}", AlienCount );

		if ( AlienCount == 0 ) {

			SpawnMother();

			Debug.Log( "All aliens dead" );
		}
	}

	private void OnEnemySpawn( EnemySpawner.Spawned eventObject ) {

		if ( eventObject.Character.Status.Info.name.Contains( "xen" ) ) {
            _enemies.Add(eventObject.Character);

			AlienCount++;

			AliensRemaining = AlienCount;
		}
	}

	private void SpawnMother() {

		_motherSpawner.Initialize();
	}

    private void KillAll()
    {
        foreach (var character in _enemies)
        {
            if (character != null)
            {
                character.Damage(1000);
            }
        }
    }
    void OnGUI() {

		if ( GUILayout.Button( "Spawn mother" ) ) {

			SpawnMother();
		}

        if (GUILayout.Button("Kill All"))
        {
            KillAll();
        }
    }

}