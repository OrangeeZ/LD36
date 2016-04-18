using System;
using System.Collections.Generic;
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

	private List<byte> _mooreNeighbourhoodVisibilityBuffer1;
	private List<byte> _mooreNeighbourhoodVisibilityBuffer2;

	private List<byte> _visibilityReadBuffer;
	private List<byte> _visibilityWriteBuffer;

	private void Start() {

		Generate();
	}

	public Bounds GetBounds() {

		return _bounds;
	}

	public void Trace( Vector3 position, int radius ) {

		Profiler.BeginSample( "WarFogSpaceMap::Clear" );
		//ClearVisible();
		Profiler.EndSample();

		var scaledRadius = Mathf.RoundToInt( radius / _cellSize );

		var startingPoint = _bounds.center - _bounds.extents;
		var relativePosition = position - startingPoint;

		var centerX = Mathf.RoundToInt( relativePosition.x / _cellSize );
		var centerY = Mathf.RoundToInt( relativePosition.z / _cellSize );

		SetPointVisible( centerY * _cellsX + centerX, true );

		UpdateMooreNeighbourhoodCache( scaledRadius + 2 );

		//_mooreNeighbourhoodVisibilityBuffer1 = _mooreNeighbourhoodVisibilityBuffer1 ?? new List<byte>();
		//_mooreNeighbourhoodVisibilityBuffer1.Capacity = GetMooreNeighbourhoodLength( scaledRadius + 1 );

		//_mooreNeighbourhoodVisibilityBuffer2 = _mooreNeighbourhoodVisibilityBuffer2 ?? new List<byte>();
		//_mooreNeighbourhoodVisibilityBuffer2.Capacity = GetMooreNeighbourhoodLength( scaledRadius + 1 );

		Profiler.BeginSample( "WarFogSpaceMap::Trace" );
		for ( var i = 1; i <= scaledRadius; ++i ) {

			//if ( i % 2 == 0 ) {

			//	_visibilityReadBuffer = _mooreNeighbourhoodVisibilityBuffer1;
			//	_visibilityWriteBuffer = _mooreNeighbourhoodVisibilityBuffer2;
			//} else {

			//	_visibilityReadBuffer = _mooreNeighbourhoodVisibilityBuffer2;
			//	_visibilityWriteBuffer = _mooreNeighbourhoodVisibilityBuffer1;
			//}

			//_visibilityWriteBuffer.Clear();

			TraceMooreNeighbourhood( centerX, centerY, i, OnTracePoint );
		}
		Profiler.EndSample();

		Profiler.BeginSample( "WarFogSpaceMap::PrepareTexture" );
	}

	public void SubmitTexture() {

		for ( var i = 0; i < _warFogColors.Length; i++ ) {

			_warFogColors[i].a = (byte)( _visibilityMap[i] * 255 );
		}

		//Blur( _warFogColors, _cellsX, _cellsZ );

		_warFogTexture.SetPixels32( _warFogColors );
		_warFogTexture.Apply();

		Profiler.EndSample();

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
		for ( var i = 0; i < _mooreNeighbourhoodLengthCache.Length; ++i ) {

			_mooreNeighbourhoodLengthCache[i] = CalculateMooreNeighbourhoodLength( i );
		}
	}

	private void TraceMooreNeighbourhood( int centerX, int centerY, int radius, Action<int, int> callback ) {

		var segmentCount = GetMooreNeighbourhoodLength( radius + 1 );
		for ( var i = 0; i < segmentCount; ++i ) {

			var rate = (float) i / segmentCount + float.Epsilon;

			var pointX = 0;//GetMooreOffsetX( radius, rate ) + centerX;
			var pointY = 0;//GetMooreOffsetY( radius, rate ) + centerY;

			GetMooreOffsets( radius, rate, out pointX, out pointY );
			pointX += centerX;
			pointY += centerY;

			if ( radius > 1 ) {

				var previousPointX = 0;// GetMooreOffsetX( radius - 1, rate ) + centerX;
				var previousPointY = 0;//GetMooreOffsetY( radius - 1, rate ) + centerY;

				GetMooreOffsets( radius - 1, rate, out previousPointX, out previousPointY );
				previousPointX += centerX;
				previousPointY += centerY;

				//var visibilityCacheIndex = Mathf.RoundToInt( (rate + 1f / segmentCount) * ( _visibilityReadBuffer.Count - 1 ) );
				var isPreviousPointVisible = GetPointVisible( previousPointY * _cellsX + previousPointX );
				if ( !isPreviousPointVisible ) {

					SetPointVisible( pointY * _cellsX + pointX, false );

					//_visibilityWriteBuffer.Add( 0 );
					continue;
				}
			}

			callback( pointX, pointY );
			//_visibilityWriteBuffer.Add( _visibilityMap[pointY * _cellsX + pointX] );
		}
	}

	private void GetMooreOffsets( int radius, float rate, out int x, out int y ) {

		var sideLength = GetMooreNeighbourhoodLength( radius ) / 4f;
		var halfSideLength = sideLength / 2f;

		var interpolatedSide = rate < 0.5f ? Mathf.Lerp( 0, sideLength, rate * 4f ) : Mathf.Lerp( 0, sideLength, 1 - ( rate - 0.5f ) * 4f );
		x = Mathf.RoundToInt( interpolatedSide - halfSideLength );

		interpolatedSide = rate < 0.75f ? Mathf.Lerp( 0, sideLength, ( rate - 0.25f ) * 4f ) : Mathf.Lerp( 0, sideLength, 1 - ( rate - 0.75f ) * 4f );

		y = Mathf.RoundToInt( interpolatedSide - halfSideLength );
	}

	//private int GetMooreOffsetX( int radius, float rate ) {

	//	var sideLength = GetMooreNeighbourhoodLength( radius ) / 4f;

	//	var interpolatedSide = rate < 0.5f ? Mathf.Lerp( 0, sideLength, rate * 4f ) : Mathf.Lerp( 0, sideLength, 1 - ( rate - 0.5f ) * 4f );

	//	var result = Mathf.RoundToInt( interpolatedSide - sideLength / 2f );

	//	return result;
	//}

	//private int GetMooreOffsetY( int radius, float rate ) {

	//	var sideLength = GetMooreNeighbourhoodLength( radius ) / 4f;

	//	var interpolatedSide = rate < 0.75f ? Mathf.Lerp( 0, sideLength, ( rate - 0.25f ) * 4f ) : Mathf.Lerp( 0, sideLength, 1 - ( rate - 0.75f ) * 4f );

	//	var result = Mathf.RoundToInt( interpolatedSide - sideLength / 2f );

	//	return result;
	//}

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

		if ( index < 0 || index >= _visibilityMap.Length ) return;

		_visibilityMap[index] = (byte) ( isVisible ? 1 : 0 );
	}

	private bool GetPointVisible( int index ) {

		if ( index < 0 || index >= _visibilityMap.Length ) return false;

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

	private static readonly float[] GaussKernel = new[] {1f / 4f, 1f / 8f, 1f / 16f};

	private static void Blur( Color[] alphamap, int width, int height ) {
		for ( var x = 1; x < width - 1; ++x ) {
			for ( var y = 1; y < height - 1; y++ ) {
				//for (var i = 0; i < numTexLayers; i++)
				{
					var blurredValue = GetBlurredValue( alphamap, x, y, 0, 0, width );

					blurredValue += GetBlurredValue( alphamap, x, y, -1, 0, width );
					blurredValue += GetBlurredValue( alphamap, x, y, 1, 0, width );
					blurredValue += GetBlurredValue( alphamap, x, y, 0, -1, width );
					blurredValue += GetBlurredValue( alphamap, x, y, 0, 1, width );

					blurredValue += GetBlurredValue( alphamap, x, y, -1, -1, width );
					blurredValue += GetBlurredValue( alphamap, x, y, 1, -1, width );
					blurredValue += GetBlurredValue( alphamap, x, y, -1, 1, width );
					blurredValue += GetBlurredValue( alphamap, x, y, 1, 1, width );

					alphamap[y * width + x] = blurredValue;
				}
			}
		}
	}

	private static Color GetBlurredValue( Color[] alphamap, int baseX, int baseY, int xOffset, int yOffset, int width ) {
		return GaussKernel[Mathf.Abs( xOffset ) + Mathf.Abs( yOffset )] * alphamap[( baseY + yOffset ) * width + baseX + xOffset];
	}

}