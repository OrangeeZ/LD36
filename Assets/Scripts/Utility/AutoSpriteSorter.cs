using UnityEngine;
using System.Collections;

public class AutoSpriteSorter : MonoBehaviour {

	[SerializeField]
	private Bounds _warFogOccluderBounds;

	[SerializeField]
	private float _warFogOccluderAngle = 45f;

	[ContextMenu( "Sort" )]
	private void Sort() {
#if UNITY_EDITOR
		var sprites = GetComponentsInChildren<SpriteRenderer>();
		foreach ( var each in sprites ) {

			each.sortingOrder = Mathf.RoundToInt( -each.transform.position.z * 100f );

			if ( each.name.ToLower().Contains( "wall" ) ) {
				
				DestroyImmediate( each.GetComponent<WarFogOccluder>() );

				var occluder = each.gameObject.AddComponent<WarFogOccluder>();
				occluder.SetLocalBounds( _warFogOccluderBounds );
				occluder.SetAdditionalAngle( _warFogOccluderAngle );
			}

			UnityEditor.EditorUtility.SetDirty( each );
		}
#endif
	}

}