using UnityEngine;
using System.Collections;
using CharacterFramework.Core;
using UnityEngine.ScriptableObjectWizard;

[Category( "Weapons" )]
public class MeleeWeaponInfo : WeaponInfo {

	private class MeleeWeapon : Weapon<MeleeWeaponInfo> {

		//public SimpleSphereCollider sphereCollider;

		private float nextAttackTime;

		public MeleeWeapon( MeleeWeaponInfo info )
			: base( info ) {

		}

		public override void Attack( Character target ) {

			if ( Time.timeSinceLevelLoad < nextAttackTime ) {

				return;
			}

			if ( target == null ) {

				return;
			}

			nextAttackTime = Time.timeSinceLevelLoad + typedInfo.baseAttackSpeed;

			//if ( character.Status.GetHitChance() < 100.Random() ) {

			//	Debug.Log( "Miss!" );

			//	return;
			//}

			target.health.Value -= typedInfo.baseDamage;

			//if ( character.Status.GetCriticalHitChance( target.Status ) > 100.Random() ) {

			//	Debug.Log( "Critical hit!" );

			//	target.Damage( character.Status.GetMeleeAttack( info.damage ) * 5 / 3, ignoreArmor: true );
			//} else {

			//	target.Damage( character.Status.GetMeleeAttack( info.damage ) );
			//}

			//Debug.Log( character.Status.GetAttackDelay( info.attackDuration ) );
		}

		public override bool CanAttack( Character target ) {

			//return ( target.pawn.position - character.pawn.position ).sqrMagnitude <= typedInfo.attackRange.Pow( 2 );
			return false;
		}
	}

	public override Item GetItem() {

		return new MeleeWeapon( this );
	}
}
