using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBubblePopup : MonoBehaviour {

	[SerializeField]
	private Text _message;

	[SerializeField]
	private float _secondsPerStringChar;

	private Transform _target;
	private float _duration;

	public void Initialize( Transform target, string text ) {

		_target = target;
		_duration = text.Length * _secondsPerStringChar;
		_message.text = text;
	}

	// Update is called once per frame
	private void Update() {

		if ( _target == null ) {

			return;
		}

		var screenPosition = Camera.main.WorldToScreenPoint( _target.position );
		transform.position = screenPosition;

		_duration -= Time.deltaTime;
		if ( _duration < 0 ) {
			
			Destroy( gameObject );
		}
	}

}