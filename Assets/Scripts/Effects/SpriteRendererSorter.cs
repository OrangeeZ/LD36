using UnityEngine;
using System.Collections;

public class SpriteRendererSorter : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	// Update is called once per frame
	private void Update() {

		if ( !transform.hasChanged ) {

			return;
		}

		transform.hasChanged = false;

		_spriteRenderer.sortingOrder = Mathf.RoundToInt( -_spriteRenderer.transform.position.z * 100f );
	}

	void Reset() {

		_spriteRenderer = GetComponent<SpriteRenderer>();
	}
}