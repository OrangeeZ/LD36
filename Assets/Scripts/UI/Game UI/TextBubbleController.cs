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
		EventSystem.Events.SubscribeOfType<Character.Speech>( OnCharacterSpeak );
	}
	
	private void OnCharacterSpeak( Character.Speech speech) {

		if ( Camera.main == null || speech.Character.Pawn == null ) {

			return;
		}

		string text;

		if (!string.IsNullOrEmpty(speech.messageId)) {
			text = _messages.GetText(speech.messageId);
		} else {
			text = _messages.GetRandom();
		}

		var instance = Instantiate (_textBubble);
		instance.transform.SetParent( transform );
		instance.gameObject.SetActive(true);
		instance.Initialize( speech.Character.Pawn.transform, text );
	}
}

