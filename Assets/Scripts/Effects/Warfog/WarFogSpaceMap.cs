using System;
using System.Linq;
using UnityEngine;

public class WarFogSpaceMap : MonoBehaviour {

	private byte[] _spaceMap;
	private byte[] _visibilityMap;

	[SerializeField]
	[Range( 0.2f, 10 )]
	private float _cellSize;

	[SerializeField]
	private Bounds _bounds;

	private WarFogOccluder[] _occluders;

	[SerializeField]
	private int _cellsX;

	[SerializeField]
	private int _cellsZ;

	[SerializeField]
	private Texture2D _warFogTexture;

	private Color32[] _warFogColors;

	private int[] _mooreNeighbourhoodLengthCache;

	void Start() {
		
		Generate();
	}

	public Bounds GetBounds() {

		return _bounds;
	}

	public void Trace( Vector3 position, int radius ) {

		ClearVisible();

		var scaledRadius = Mathf.RoundToInt( radius / _cellSize );

		var startingPoint = _bounds.center - _bounds.extents;
		var relativePosition = position - startingPoint;

		var centerX = Mathf.RoundToInt( relativePosition.x / _cellSize );
		var centerY = Mathf.RoundToInt( relativePosition.z / _cellSize );

		SetPointVisible( centerY * _cellsX + centerX, true );

		for ( var i = 1; i <= scaledRadius; ++i ) {

			TraceMooreNeighbourhood( centerX, centerY, i, OnTracePoint );
		}

		for ( var i = 0; i < _warFogColors.Length; i++ ) {

			_warFogColors[i].a = (byte) ( _visibilityMap[i] * 255 );
		}

		_warFogTexture.SetPixels32( _warFogColors );
		_warFogTexture.Apply();

		if ( WarFogPostEffectRenderer.Instance != null ) {

			WarFogPostEffectRenderer.Instance.SetTexture( this, _warFogTexture );
		}
	}

	[ContextMenu( "Generate" )]
	private void Generate() {

		_occluders = FindObjectsOfType<WarFogOccluder>();

		var startingPoint = _bounds.center - _bounds.extents;
		_cellsX = Mathf.RoundToInt( _bounds.size.x / _cellSize );
		_cellsZ = Mathf.RoundToInt( _bounds.size.z / _cellSize );

		if ( _warFogTexture != null ) {

			DestroyImmediate( _warFogTexture );
		}

		_warFogTexture = new Texture2D( _cellsX, _cellsZ, TextureFormat.Alpha8, mipmap: false );
		_warFogColors = _warFogTexture.GetPixels32();

		_spaceMap = new byte[_cellsX * _cellsZ];
		_visibilityMap = new byte[_cellsX * _cellsZ];

		for ( var x = 0; x < _cellsX; ++x ) {

			for ( var z = 0; z < _cellsZ; ++z ) {

				var currentPoint = startingPoint + Vector3.forward * z * _cellSize + Vector3.right * x * _cellSize;
				_spaceMap[z * _cellsX + x] = IsPointOccluded( currentPoint ) ? (byte) 1 : (byte) 0;
			}
		}
	}

	private void UpdateMooreNeighbourhoodCache( int requiredRadius ) {

		if ( _mooreNeighbourhoodLengthCache != null && requiredRadius < _mooreNeighbourhoodLengthCache.Length ) {

			return;
		}

		_mooreNeighbourhoodLengthCache = new int[requiredRadius + 1];
		for ( var i = 0; i < requiredRadius; ++i ) {

			_mooreNeighbourhoodLengthCache[i] = CalculateMooreNeighbourhoodLength( i );
		}
	}

	private void TraceMooreNeighbourhood( int centerX, int centerY, int radius, Action<int, int> callback ) {

		UpdateMooreNeighbourhoodCache( radius + 1 );
		var segmentCount = GetMooreNeighbourhoodLength( radius + 1 );
		for ( var i = 0; i < segmentCount; ++i ) {

			var rate = (float) i / segmentCount + float.Epsilon;

			var pointX = GetMooreOffsetX( radius, rate ) + centerX;
			var pointY = GetMooreOffsetY( radius, rate ) + centerY;

			var previousPointX = GetMooreOffsetX( radius - 1, rate ) + centerX;
			var previousPointY = GetMooreOffsetY( radius - 1, rate ) + centerY;

			var isPreviousPointVisible = GetPointVisible( previousPointY * _cellsX + previousPointX );
			if ( !isPreviousPointVisible ) {

				SetPointVisible( pointY * _cellsX + pointX, false );

				continue;
			}

			callback( pointX, pointY );
		}
	}

	private int GetMooreOffsetX( int radius, float rate ) {

		var sideLength = GetMooreNeighbourhoodLength( radius ) / 4f;

		var result = rate < 0.5f ? Mathf.Lerp( 0, sideLength, rate * 4f ) : Mathf.Lerp( 0, sideLength, 1 - ( rate - 0.5f ) * 4f );

		return Mathf.RoundToInt( result - sideLength / 2f );
	}

	private int GetMooreOffsetY( int radius, float rate ) {

		var sideLength = GetMooreNeighbourhoodLength( radius ) / 4f;

		var result = rate < 0.75f ? Mathf.Lerp( 0, sideLength, ( rate - 0.25f ) * 4f ) : Mathf.Lerp( 0, sideLength, 1 - ( rate - 0.75f ) * 4f );

		return Mathf.RoundToInt( result - sideLength / 2f );
	}

	private int CalculateMooreNeighbourhoodLength( int radius ) {

		return GetMooreNeighbourhoodCount( radius ) - GetMooreNeighbourhoodCount( radius - 1 );
	}

	private int GetMooreNeighbourhoodLength( int radius ) {

		if ( radius < 0 ) {

			return 0;
		}

		return _mooreNeighbourhoodLengthCache[radius];
	}

	private int GetMooreNeighbourhoodCount( int radius ) {

		if ( radius < 0 ) {

			return 0;
		}

		return (int) Mathf.Pow( 2 * radius + 1, 2 );
	}

	private void OnTracePoint( int x, int y ) {

		//var isVisible = true;
		var isOccluded = GetSpaceMapPointOccluded( y * _cellsX + x );

		//if ( !isOccluded ) {

		//	var isVisited = GetPointVisited( y * _cellsX + x );

		//	isVisible = isVisited;
		//} else {

		//	isVisible = false;
		//}

		SetPointVisible( y * _cellsX + x, !isOccluded );
	}

	private bool IsPointOccluded( Vector3 point ) {

		return _occluders.Any( _ => _.IsAffectingPoint( point ) );
	}

	private void ClearVisible() {

		for ( var i = 0; i < _spaceMap.Length; ++i ) {

			SetPointVisible( i, false );
		}
	}

	private void SetPointVisible( int index, bool isVisible ) {

		_visibilityMap[index] = (byte) ( isVisible ? 1 : 0 );
	}

	private bool GetPointVisible( int index ) {

		return _visibilityMap[index] == 1;
	}

	private bool GetSpaceMapPointOccluded( int index ) {

		if ( index < 0 || index > _spaceMap.Length ) {
			return false;
		}

		return _spaceMap[index] == 1;
	}

	private void OnDrawGizmos() {

		Gizmos.DrawWireCube( _bounds.center, _bounds.size );

		if ( _spaceMap == null ) {

			return;
		}

		var startingPoint = _bounds.center - _bounds.extents;
		var cellsX = Mathf.RoundToInt( _bounds.size.x / _cellSize );
		var cellsZ = Mathf.RoundToInt( _bounds.size.z / _cellSize );

		for ( var x = 0; x < cellsX; ++x ) {

			for ( var z = 0; z < cellsZ; ++z ) {

				var index = z * cellsX + x;
				var isOccluded = GetSpaceMapPointOccluded( index ); //_spaceMap[z * cellsX + x] > 0;
				var isVisible = GetPointVisible( index );

				var color = isOccluded ? new Color( 1, 0, 0, 0.5f ) : new Color( 0, 1, 0, 0.5f );
				if ( !isVisible ) {

					color.a = 0.1f;
				}

				Gizmos.color = color;

				var currentPoint = startingPoint + Vector3.forward * z * _cellSize + Vector3.right * x * _cellSize;

				Gizmos.DrawCube( currentPoint, Vector3.one * _cellSize );
			}
		}
	}

}