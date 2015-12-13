using UnityEngine;
using System.Collections;
using csv;

public abstract class RangedWeaponBehaviourInfo : ScriptableObject {

	public abstract RangedWeaponBehaviour GetBehaviour();

}

public abstract class RangedWeaponBehaviour {

	public virtual bool IsReloading { get; protected set; }

	public abstract void Initialize( IInventory ownerInventory, RangedWeaponInfo info );

	public abstract void TryShoot();

}