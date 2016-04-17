using UnityEngine;
using System.Collections;

public class CharacterSpriteAnimationController : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite[] _sprites;

	private int _currentIndex;

	public void UpdateDirection( int directionX, int directionY ) {
		
		_currentIndex = ( directionY + 1 ) * 3 + ( directionX + 1 );

		_spriteRenderer.sprite = _sprites[_currentIndex];
	}

}