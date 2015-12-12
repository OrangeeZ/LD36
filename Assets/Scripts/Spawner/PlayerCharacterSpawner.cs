using System;
using System.Linq;
using Packages.EventSystem;
using UnityEngine;

public class PlayerCharacterSpawner : MonoBehaviour {

	//public Action<Character> Spawned;

	public class Spawned : IEventBase {

		public Character Character;

	}

	public CharacterInfo characterInfo;

	public ItemInfo[] startingItems;

	public WeaponInfo startingWeapon;

	public CharacterStatusEffectInfo startingStatusEffect;

	public CameraBehaviour cameraBehaviour;

	public bool isPlayerCharacter = false;

	private Character character;

	public void Initialize() {

		character = characterInfo.GetCharacter( startingPosition: transform.position );

		foreach ( var each in startingItems.Select( _ => _.GetItem() ) ) {

			character.Inventory.AddItem( each );
		}

		if ( startingWeapon != null ) {

			var weapon = startingWeapon.GetItem();

			character.Inventory.AddItem( weapon );
			weapon.Apply();
		}

		if ( cameraBehaviour != null ) {

			var cameraBehaviourInstance = Instantiate( cameraBehaviour );
			cameraBehaviourInstance.transform.position = transform.position;
			cameraBehaviourInstance.SetTarget( character.Pawn );
		}

		if ( isPlayerCharacter ) {

			//GameScreen.instance.statsPanel.SetCharacter( character );
		}

		character.Status.AddEffect( startingStatusEffect );

		EventSystem.RaiseEvent( new Spawned { Character = character } );

		//if ( Spawned != null ) {

		//    Spawned( character );
		//}
	}

}