using System;
using CharacterFramework.Core;
using UnityEngine;

public abstract class Weapon : Item {

	protected Action attackCallback;

	protected Vector3 direction;

	protected Weapon( ItemInfo info )
		: base( info ) {
	}

	public virtual void Setup( Character ownerCharacter, Vector3 direction, Action attackCallback ) {

		this.character = ownerCharacter;
		this.direction = direction;
		this.attackCallback = attackCallback;
	}

	public virtual WeaponInfo GetInfo() {

		return null;
	}

	public virtual void Attack( Character target ) {}

    public virtual void Attack( Vector3 direction ) {}

    public virtual bool CanAttack( Character target ) {

        return false;
    }

	public override void Apply() {

		//character.inventory.SetArmSlotItem( ( info as WeaponInfo ).slotType, this );
	}
}

public abstract class Weapon<T> : Weapon where T : WeaponInfo {

	protected readonly T typedInfo;

	protected Weapon( ItemInfo info )
		: base( info ) {

		this.typedInfo = info as T;
	}
}