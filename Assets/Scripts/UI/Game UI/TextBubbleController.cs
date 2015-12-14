using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class TextBubbleController : MonoBehaviour
{
	[SerializeField]
	protected TextBubblePopup _textBubble;
	[SerializeField]
	protected MessagesInfo _messages;

	void Start ()
	{
		EventSystem.Events.SubscribeOfType<Character.Speach>( OnCharacterSpeak );
	}
	
	private void OnCharacterSpeak( Character.Speach speach) {

		if ( Camera.main == null || speach.Character.Pawn == null ) {

			return;
		}

		string text;

		if (!string.IsNullOrEmpty(speach.messageId)) {
			text = _messages.GetText(speach.messageId);
		} else {
			text = _messages.GetRandom();
		}

		var instance = Instantiate (_textBubble);
		instance.transform.SetParent( transform );
		instance.gameObject.SetActive(true);
		instance.Initialize( speach.Character.Pawn.transform, text );
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

