using System;
using System.Linq;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour {

    public Action<Character> Spawned;

    public CharacterInfo characterInfo;

    public ItemInfo[] startingItems;

    public WeaponInfo startingWeapon;

    public CameraBehaviour cameraBehaviour;

    public bool isPlayerCharacter = false;

    private Character character;

    private void Start() {

        character = characterInfo.GetCharacter( startingPosition: transform.position );

        foreach ( var each in startingItems.Select( _ => _.GetItem() ) ) {

            character.inventory.AddItem( each );
        }

        if ( startingWeapon != null ) {

            var weapon = startingWeapon.GetItem();

            character.inventory.AddItem( weapon );
            weapon.Apply();
        }

        if ( cameraBehaviour != null ) {

            var cameraBehaviourInstance = Instantiate( cameraBehaviour );
            cameraBehaviourInstance.transform.position = transform.position;
            cameraBehaviourInstance.SetTarget( character.pawn );
        }

        if ( isPlayerCharacter ) {

            //GameScreen.instance.statsPanel.SetCharacter( character );
        }

        if ( Spawned != null ) {

            Spawned( character );
        }
    }

}