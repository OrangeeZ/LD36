using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEngine.Events;

public class RoomDevice : MonoBehaviour {

	public RoomDeviceInfo RoomDeviceInfo;

	public UnityEvent OnInteract;

	private float _health;
	private float _cooldown;

	private RoomEffect _roomEffect;

	private void Start() {

		SetFixed();

		var effectName = "RoomEffect" + RoomDeviceInfo.Effect;
		Debug.Log( effectName );

		var roomEffectType = Types.GetType( effectName, Assembly.GetCallingAssembly().FullName );

		_roomEffect = (RoomEffect) Activator.CreateInstance( roomEffectType );
		_roomEffect.EffectValue = RoomDeviceInfo.EffectValue;
	}

	private void Update() {

		if ( _cooldown > 0 ) {

			_cooldown -= Time.deltaTime;
		} else {

			if ( !RoomDeviceInfo.CanBeActive ) {

				_roomEffect.Activate();
			}
		}
	}

	public void Damage( float value ) {

		if ( !RoomDeviceInfo.CanBeBroken ) {

			return;
		}

		_health -= value;
	}

	public bool IsBroken() {

		return RoomDeviceInfo.CanBeBroken && _health <= 0;
	}

	public void SetFixed() {

		_health = RoomDeviceInfo.Durability;
	}

	public bool IsInteractive() {

		return RoomDeviceInfo.CanBeActive && _cooldown <= 0;
	}

	public void Interact() {

		if ( !IsInteractive() ) {
			return;
		}

		var isSupercharge = CheckSupercharge();

		if ( isSupercharge ) {

			_cooldown = RoomDeviceInfo.SuperchargeTime * RoomDeviceInfo.SuperchargeHits;

			StartCoroutine( SuperchargeLoop() );
		} else {

			_cooldown = RoomDeviceInfo.RechargeTime;

			_roomEffect.Activate();
		}
	}

	private IEnumerator SuperchargeLoop() {

		for ( var i = 0; i < RoomDeviceInfo.SuperchargeHits; i++ ) {

			_roomEffect.Activate();

			yield return new WaitForSeconds( RoomDeviceInfo.SuperchargeTime );
		}
	}

	private bool CheckSupercharge() {

		if ( !RoomDeviceInfo.HasSupercharge ) {

			return false;
		}

		var chance = 1f.Random();

		if ( chance < RoomDeviceInfo.SuperchargeChance ) {

			return true;
		}

		return false;
	}

}