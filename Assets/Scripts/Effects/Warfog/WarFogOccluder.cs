using UnityEngine;
using System.Collections;

public class WarFogOccluder : MonoBehaviour {

	[SerializeField]
	private Bounds _bounds;

	private void Reset() {

		var renderer = GetComponentInChildren<Renderer>( includeInactive: true );
		if ( renderer != null ) {

			_bounds = renderer.bounds;
		} else {

			var collider = GetComponentInChildren<Collider>( includeInactive: true );
			if ( collider != null ) {

				_bounds = collider.bounds;
			}
		}
	}

	public bool IsAffectingPoint( Vector3 point )
	{
	    var localPoint = transform.worldToLocalMatrix.MultiplyPoint3x4(point);
        localPoint.Scale(transform.localScale);
        return _bounds.Contains(localPoint);
	}

}