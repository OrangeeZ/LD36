using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu( menuName = "Create/Weapon/Behaviours/Acorn" )]
public class AcornRangedBehaviourInfo : RangedWeaponBehaviourInfo {

	private class AcornRangedBehaviour : RangedWeaponBehaviour {

		private float _nextAttackTime;
		private RangedWeaponInfo _info;
		private IInventory _ownerInventory;

		public override bool IsReloading {
			get { return Time.timeSinceLevelLoad < _nextAttackTime || _ownerInventory.GetItemCount<AcornAmmoItemInfo.AcornAmmo>() == 0; }
			protected set { }
		}

		public override void Initialize( IInventory ownerInventory, RangedWeaponInfo info ) {

			_info = info;
			_ownerInventory = ownerInventory;
		}

		public override void TryShoot() {

			if ( !IsReloading ) {
				
				_nextAttackTime = Time.timeSinceLevelLoad + _info.BaseAttackSpeed;

				_ownerInventory.RemoveItem<AcornAmmoItemInfo.AcornAmmo>();
			}
		}

	}

	public override RangedWeaponBehaviour GetBehaviour() {
		
		return new AcornRangedBehaviour();
	}

}
