using UnityEngine;
using System.Collections;

public class AutoSpriteSorter : MonoBehaviour {

	[ContextMenu( "Sort" )]
	private void Sort() {
#if UNITY_EDITOR
		var sprites = GetComponentsInChildren<SpriteRenderer>();
		foreach ( var each in sprites ) {

			each.sortingOrder = Mathf.RoundToInt( -each.transform.position.z * 100f );

			UnityEditor.EditorUtility.SetDirty( each );
		}
#endif
	}

}