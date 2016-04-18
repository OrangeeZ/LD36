using UnityEngine;
using System.Collections;
using Assets.Scripts.Level;
using Packages.EventSystem;
using UniRx;

public class SoundEffectController : MonoBehaviour {

	[SerializeField]
	private AudioClip _doorOpen;

	[SerializeField]
	private AudioClip _doorClose;

	// Use this for initialization
	private void Start() {

		EventSystem.Events.SubscribeOfType<DoorAnimationTrigger.StateChange>( OnDoorStateChange );
	}

	private void OnDoorStateChange( DoorAnimationTrigger.StateChange eventObject ) {

		AudioSource.PlayClipAtPoint( eventObject.Trigger.IsOpen ? _doorOpen : _doorClose, eventObject.Trigger.transform.position );
	}

	// Update is called once per frame
	private void Update() {

	}

}