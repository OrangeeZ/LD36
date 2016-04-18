﻿using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class RoomBasedDebuffController : MonoBehaviour {

	[SerializeField]
	private PlayerCharacterSpawner _playerCharacterSpawner;

	// Use this for initialization
	private void Start() {

		EventSystem.Events.SubscribeOfType<Room.EveryoneDied>( OnEveryoneDieInRoom );
	}

	private void OnEveryoneDieInRoom( Room.EveryoneDied everyoneDiedEvent ) {

		var playerCharacter = _playerCharacterSpawner.character;

		switch ( everyoneDiedEvent.Room.GetRoomType() ) {

			case Room.Type.Workshop:
				var weapon = playerCharacter.Inventory.GetArmSlotItem( ArmSlotType.Primary ) as RangedWeaponInfo.RangedWeapon;
				weapon.ReloadDurationScale = 2f;
				break;
		}
	}

}