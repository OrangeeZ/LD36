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

	[SerializeField]
	private AudioClip _roomPowerDown;

	[SerializeField]
	private AudioClip _healbotHealSuccess;

	[SerializeField]
	private AudioClip _healbotHealFailed;

	// Use this for initialization
	private void Start() {

		EventSystem.Events.SubscribeOfType<DoorAnimationTrigger.StateChange>( OnDoorStateChange );
		EventSystem.Events.SubscribeOfType<Room.EveryoneDied>( OnEveryoneDieInRoom );

		EventSystem.Events.SubscribeOfType<HealbotObject.TriedHeal>( OnTryHeal );
	}

	private void OnTryHeal( HealbotObject.TriedHeal eventObject ) {

		AudioSource.PlayClipAtPoint( eventObject.DidSucceed ? _healbotHealSuccess : _healbotHealFailed, eventObject.Healbot.position );
	}

	private void OnEveryoneDieInRoom( Room.EveryoneDied eventObject ) {

		if ( eventObject.Room.GetRoomType() != Room.RoomType.Default ) {

			AudioSource.PlayClipAtPoint( _roomPowerDown, Vector3.zero );
		}
	}

	private void OnDoorStateChange( DoorAnimationTrigger.StateChange eventObject ) {

		AudioSource.PlayClipAtPoint( eventObject.Trigger.IsOpen ? _doorOpen : _doorClose, eventObject.Trigger.transform.position );
	}

	// Update is called once per frame
	private void Update() {

	}

}