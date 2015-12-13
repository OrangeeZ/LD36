using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Create/Weapon/Behaviours/Default")]
public class DefaultRangedBehaviourInfo : RangedWeaponBehaviourInfo {

	private class DefaultRangedBehaviour : RangedWeaponBehaviour {

		public int AmmoInClip { get; private set; }

		public override bool IsReloading {

			get {

				if ( _isReloading && Time.timeSinceLevelLoad > _nextAttackTime ) {

					_isReloading = false;
				}

				return _isReloading;
			}

			protected set { _isReloading = value; }
		}

		private bool _isReloading;
		private float _nextAttackTime;
		private RangedWeaponInfo.RangedWeapon _ownerWeapon;

		public override void Initialize( IInventory ownerInventory, RangedWeaponInfo.RangedWeapon ownerWeapon ) {

			_ownerWeapon = ownerWeapon;

			AmmoInClip = _ownerWeapon.ClipSize;
		}

		public override void TryShoot() {

			AmmoInClip--;

			if ( AmmoInClip == 0 ) {

				AmmoInClip = _ownerWeapon.ClipSize;

				_nextAttackTime = Time.timeSinceLevelLoad + _ownerWeapon.ReloadDuration;

				IsReloading = true;
			} else {

				_nextAttackTime = Time.timeSinceLevelLoad + _ownerWeapon.BaseAttackSpeed;
			}
		}

	}

	public override RangedWeaponBehaviour GetBehaviour() {
		
		return new DefaultRangedBehaviour();
	}

}
