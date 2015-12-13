using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

	[SerializeField]
	private Text _value;

	[SerializeField]
	private float _distanceDelta;

	[SerializeField]
	private float _duration;

	public void Initialize( float damage ) {

		_value.text = damage.ToString();

		StartCoroutine( PlayAnimation() );
	}

	private IEnumerator PlayAnimation() {

		var from = transform.position;
		var to = transform.position + Vector3.up * _distanceDelta;

		var fromColor = _value.color;
		var toColor = _value.color;
		toColor.a = 0;

		var timer = new AutoTimer( _duration );
		while ( timer.ValueNormalized < 1 ) {

			transform.position = Vector3.Lerp( from, to, timer.ValueNormalized );
			_value.color = Color.Lerp( fromColor, toColor, timer.ValueNormalized );

			yield return null;
		}

		Destroy( gameObject );
	}
}
