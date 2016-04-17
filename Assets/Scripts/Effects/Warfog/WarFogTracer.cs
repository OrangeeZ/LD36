using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFogTracer : MonoBehaviour {

	[SerializeField]
	private float _radius = 5f;

	private WarFogSpaceMap _warFogSpaceMap;

	private struct Point {

		public readonly int X;
		public readonly int Y;

		public Point( int x, int y ) {

			X = x;
			Y = y;
		}

	}

	private IEnumerable<Point> GetMooreNeighbourhood( int radius ) {

		var y = -radius;
		var x = -radius;

		for ( ; x <= radius; ++x ) {

			yield return new Point( x, y );
		}

		for ( ; y <= radius; ++y ) {

			yield return new Point( x, y );
		}

		for ( ; x >= -radius; --x ) {

			yield return new Point( x, y );
		}

		for ( ; y >= -radius; --y ) {

			yield return new Point( x, y );
		}
	}

	private void OnDrawGizmos() {

		//for ( var r = 0; r < Mathf.RoundToInt( _radius ); r++ ) {

		//	foreach ( var each in GetMooreNeighbourhood( r ) ) {

		//		Gizmos.DrawSphere( new Vector3( transform.position.x + each.X, 0, transform.position.z + each.Y ), 0.1f );
		//	}
		//}

		_warFogSpaceMap = _warFogSpaceMap ?? FindObjectOfType<WarFogSpaceMap>();

		_warFogSpaceMap.Trace( transform.position, Mathf.RoundToInt( _radius ) );
	}

}