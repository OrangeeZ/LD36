using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/Items/Healing item" )]
public class HealingItemInfo : ItemInfo {

	public int healingAmount = 1;

	public class HealingItem : Item {

		public HealingItem( ItemInfo info ) : base( info ) {

		}

		public override void Apply() {

			character.Health.Value += ( info as HealingItemInfo ).healingAmount;

			character.Inventory.RemoveItem( this );
		}
	}

	public override Item GetItem() {

		return new HealingItem( this );
	}

	//public override void Apply( Character target ) {

	//	target.health.Value += healingAmount;
	//}
}
