using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class RoomDevice : MonoBehaviour {

	public enum State {

		Ready,
		Active,
		Cooldown,
		Broken,
		Supercharge,

	}

	public State CurrentState;

	public UnityEvent OnInteract;

	public void SetFixed() {

		CurrentState = State.Active;
	}

	public void Interact() {

		if ( OnInteract != null ) {
			
			OnInteract.Invoke();
		}
	}

}