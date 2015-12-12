using System;
using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

[HideInWizard]
public class WeaponInfo : ItemInfo {

	public int baseDamage;

	public float baseAttackSpeed;

	public float attackRange = 2f;

	public ArmSlotType slotType = ArmSlotType.Primary;
}
