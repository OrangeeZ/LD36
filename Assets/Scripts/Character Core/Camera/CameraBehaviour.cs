using System;
using UnityEngine;
using System.Collections;
using CharacterFramework.Core;

public class CameraBehaviour : MonoBehaviour {

	protected CharacterPawn target;

	public void SetTarget( CharacterPawn target ) {

		this.target = target;
	}

	protected virtual void UpdateCamera() {


	}

	void LateUpdate() {

		if ( target != null ) {

			UpdateCamera();
		} else {

			throw new Exception( "Camera target is not set" );
		}
	}
}
