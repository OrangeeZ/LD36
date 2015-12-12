using System;
using UnityEngine;
using System.Collections;

public class WeaponInfo : ItemInfo {

	public int BaseDamage;

	public float BaseAttackSpeed;

	public float AttackRange = 2f;

	public ArmSlotType SlotType = ArmSlotType.Primary;
}
