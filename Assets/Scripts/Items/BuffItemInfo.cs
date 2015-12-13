using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Create/Items/Buff item info")]
public class BuffItemInfo : ItemInfo {

	[SerializeField]
	private CharacterStatusEffectInfo _statusEffectInfo;

	private class BuffItem : Item {

		private readonly BuffItemInfo _info;

		public BuffItem( BuffItemInfo info ) : base( info ) {

			_info = info;

		}

		public override void Apply() {

			_info._statusEffectInfo.Add( Character );
			//Character.Status.AddEffect( _info._statusEffectInfo );

			Character.Inventory.RemoveItem( this );
		}

	}

	public override Item GetItem() {

		return new BuffItem( this );
	}

}