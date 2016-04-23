using UnityEngine;

namespace WarFog {

	public class WarFogRenderer : MonoBehaviour {

		public static WarFogRenderer Instance { get; private set; }

		[SerializeField]
		private Shader _shader;

		private Material _material;
		private Texture2D _warFogTexture;

		private float _brightness = 1f;

		private void Awake() {

			Instance = this;

			_material = new Material( _shader );
		}

		// Use this for initialization
		private void Start() {

			SetBrightness( 1f );

			GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		}

		private void OnRenderImage( RenderTexture src, RenderTexture dest ) {

			//var blurredWarFog = _blurOptimized.BlurTexture( _warFogTexture );

			//_material.SetTexture( "_WarFogTexture", blurredWarFog );

			Graphics.Blit( src, dest, _material );
			//Graphics.Blit( blurredWarFog, dest, _material );

			//RenderTexture.ReleaseTemporary( blurredWarFog );
		}

//		public void SetTexture( WarFogSpaceMap spaceMap, Texture2D warFogTexture ) {
//
//			_warFogTexture = warFogTexture;
//
//			_material.SetTexture( "_WarFogTexture", _warFogTexture );
//
//			_material.SetFloat( "_WarFogBrightness", _brightness );
//
//			var spaceMapBounds = spaceMap.GetBounds();
//			_material.SetMatrix( "_World2Texture", Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( 1f / spaceMapBounds.size.x, 0, 1f / spaceMapBounds.size.z ) ) );
//
//			var inverseViewProjectionMatrix = ( Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix ).inverse;
//			_material.SetMatrix( "_ViewProjectInverse", inverseViewProjectionMatrix );
//
//			_material.SetMatrix( "_Camera2World", Camera.main.cameraToWorldMatrix );
//		}

		public void SetTexture( DistanceField distanceField, Texture2D warFogTexture ) {

			_warFogTexture = warFogTexture;



			_material.SetTexture( "_WarFogTexture", _warFogTexture );

			_material.SetFloat( "_WarFogBrightness", _brightness );
			_material.SetFloat( "_MaxFieldDistance", distanceField.GetMaxFieldDistance());

			var spaceMapBounds = distanceField.GetBounds();
			_material.SetMatrix( "_World2Texture", Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( 1f / spaceMapBounds.size.x, 0, 1f / spaceMapBounds.size.z ) ) );

			var inverseViewProjectionMatrix = ( Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix ).inverse;
			_material.SetMatrix( "_ViewProjectInverse", inverseViewProjectionMatrix );

			_material.SetMatrix( "_Camera2World", Camera.main.cameraToWorldMatrix );
		}

		public void SetTracerPosition( Vector3 position ) {

			_material.SetVector( "_WorldTracerPosition", position );
		}

		public void SetBrightness( float value ) {

			_brightness = value;
		}

	}

}