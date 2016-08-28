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

	public RoomInfo RoomInfo;

	public State CurrentState;

	public UnityEvent OnInteract;

	private float _health;
	private float _cooldown;

	private void Start() {

		SetFixed();
	}

	private void Update() {
		
		if ( _cooldown > 0 ) {

			_cooldown -= Time.deltaTime;
		}
	}

	public void Damage( float value ) {

		if ( !RoomInfo.CanBeBroken ) {

			return;
		}

		_health -= value;
	}

	public bool IsBroken() {

		return RoomInfo.CanBeBroken && _health <= 0;
	}

	public void SetFixed() {

		_health = RoomInfo.Durability;
	}

	public void Interact() {

		if ( RoomInfo.CanBeActive && _cooldown <= 0 ) {

			_cooldown = RoomInfo.RechargeTime;

			//Activate skill
		}
	}

}