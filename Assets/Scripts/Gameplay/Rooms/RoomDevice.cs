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

	[SerializeField]
	private float _timeUntilBroken;

	private float _brokenProgress;

	public void SetFixed() {

		_brokenProgress = 0f;
	}

	private void Update() {

		_brokenProgress += Time.deltaTime;
	}

	public void Interact() {

		if ( OnInteract != null ) {
			
			OnInteract.Invoke();
		}
	}

}