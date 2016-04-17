using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFogTracer : MonoBehaviour {

	[SerializeField]
	private float _radius = 5f;

	private WarFogSpaceMap _warFogSpaceMap;

	void OnDrawGizmos() {

		if ( !Application.isPlaying ) {

			Update();
		}
	}

	void Update() {

		_warFogSpaceMap = _warFogSpaceMap ?? FindObjectOfType<WarFogSpaceMap>();

		_warFogSpaceMap.Trace( transform.position, Mathf.RoundToInt( _radius ) );
	}

}