using UnityEngine;
using System.Collections;
using System;

public class NPCView : AObject {

	public Transform itemSpawnPoint;
	public NPCInfo npc;
	public bool destroyOnPickup;

	public ItemView itemView { get; private set; }

	private void Start() {

		itemView = npc.itemInfo.DropItem( itemSpawnPoint ?? transform );
		itemView.giver = this;
	}

	public void OnPickedUp( Character character ) {

		character.Pawn.AddLevel( npc.CharacterScaleBonus );

		var playerCharacterStatusInfo = character.Status.Info as PlayerCharacterStatusInfo;
		if ( playerCharacterStatusInfo != null ) {

			character.Status.ModifierCalculator.Add( ModifierType.BaseMoveSpeed, new OffsetValue( playerCharacterStatusInfo.SpeedBonusPerItem, OffsetValue.OffsetValueType.Constant ) );
		}

		if ( destroyOnPickup ) {

			Destroy( gameObject );
		}
	}

}