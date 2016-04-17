﻿using UnityEngine;
using System.Collections;

public class WarFogPostEffectRenderer : MonoBehaviour {

	public static WarFogPostEffectRenderer Instance { get; private set; }

	[SerializeField]
	private Shader _shader;

	private Material _material;

	private void Awake() {

		Instance = this;
	}

	// Use this for initialization
	private void Start() {

		_material = new Material( _shader );
	}

	private void OnRenderImage( RenderTexture src, RenderTexture dest ) {

		Graphics.Blit( src, dest, _material );
	}

	public void SetTexture( WarFogSpaceMap spaceMap, Texture2D warFogTexture ) {

		_material.SetTexture( "_WarFogTexture", warFogTexture );

		var spaceMapBounds = spaceMap.GetBounds();
		_material.SetMatrix( "_World2Texture", Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( 1f / spaceMapBounds.size.x, 0, 1f / spaceMapBounds.size.z ) ) );

		var inverseViewProjectionMatrix = ( Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix ).inverse;
		_material.SetMatrix( "_ViewProjectInverse", inverseViewProjectionMatrix );

		_material.SetMatrix( "_Camera2World", Camera.main.cameraToWorldMatrix );
	}

}