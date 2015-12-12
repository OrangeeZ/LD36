using UnityEngine;
using System.Collections;

namespace CharacterFramework.Core {

	public class ItemView : AObject {

		public Item item;

		public float fadeInDuration = 1f;

		public AnimationCurve scaleCurve;
		public AnimationCurve positionCurve;

		private IEnumerator Start() {

			var timer = new AutoTimer( fadeInDuration );

			var currentPosition = localPosition;

			while ( timer.ValueNormalized < 1f ) {

				transform.localScale = Vector3.one * scaleCurve.Evaluate( timer.ValueNormalized );
				//transform.localPosition = currentPosition + transform.up * positionCurve.Evaluate( timer.ValueNormalized );

				yield return null;
			}
		}

		public void NotifyPickUp( Character character ) {

			Destroy( gameObject );
		}

		public void SetColor( Color baseColor ) {

			var renderers = GetComponentsInChildren<Renderer>();
			foreach ( var each in renderers ) {

				each.material.SetColor( "_Color", baseColor );
			}
		}

	}

}