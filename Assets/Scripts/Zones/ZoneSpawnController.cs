using UnityEngine;
using System.Collections;

public class ZoneSpawnController : MonoBehaviour {

	[SerializeField]
	private ZoneSpawner[] _zoneSpawners;

	private ZoneSpawner _currentZone;

	[SerializeField]
	private float _autoDrainInterval;

	private float _zoneTimer = 0f;

	public void Initialize() {

		enabled = true;
	}

	private void Awake() {

		enabled = false;
	}

	private void Update() {

		if ( _currentZone == null || _currentZone.IsDrained ) {

			_currentZone = _zoneSpawners.RandomElement();
			_currentZone.Initialize();

			_zoneTimer = 0f;
		}

		if ( ( _zoneTimer += Time.deltaTime ) >= _autoDrainInterval ) {

			_currentZone.StartAutoDrain();
		}
	}

	[ContextMenu( "Hook zones" )]
	private void HookZones() {

		_zoneSpawners = FindObjectsOfType<ZoneSpawner>();
	}

}