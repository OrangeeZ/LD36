using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using csv;
using Expressions;
using UniRx;
using UnityEngine.Serialization;

[CreateAssetMenu( menuName = "Create/Weapons/Ranged" )]
public class RangedWeaponInfo : WeaponInfo {

	[SerializeField]
	private Projectile _projectilePrefab;

	[SerializeField]
	private float _projectileSpeed;

	public int ClipSize;
	public float ReloadDuration;

	[SerializeField]
	private int _projectilesPerShot;

	[SerializeField]
	private float _deviationCoefficient;

	[SerializeField]
	private float _shotConeAngle;

	[SerializeField]
	private AudioClip _sound;

	[SerializeField]
	private float _projectileLifetime;

	[SerializeField]
	private RangedWeaponBehaviourInfo _weaponBehaviourInfo;

	public class RangedWeapon : Weapon<RangedWeaponInfo> {

		public int AmmoInClip { get; private set; }

		public float BaseAttackSpeed {
			get { return Character.Status.ModifierCalculator.CalculateFinalValue( ModifierType.BaseAttackSpeed, BaseAttackSpeed ); }
		}

		public int ClipSize { get; private set; }
		public float ReloadDuration { get; set; }

		private RangedWeaponBehaviour _behaviour;

		public RangedWeapon( RangedWeaponInfo info ) : base( info ) {

			ClipSize = info.ClipSize;
			ReloadDuration = info.ReloadDuration;
		}

		public override void SetCharacter( Character character ) {

			base.SetCharacter( character );

			_behaviour = typedInfo._weaponBehaviourInfo.GetBehaviour();
			_behaviour.Initialize( Inventory, this );

		}

		public override void Attack( Character target, EnemyCharacterStatusInfo statusInfo ) {

			if ( target == null || _behaviour.IsReloading ) {

				return;
			}

			if ( AttackCallback != null ) {

				AttackCallback();
			}

			for ( var i = 0; i < typedInfo._projectilesPerShot; ++i ) {

				var projectile = GetProjectileInstance();
				var targetDirection = ( target.Pawn.position - Character.Pawn.position ).Set( y: 0 ).normalized;
				var projectileDirection = GetOffsetDirection( targetDirection, i );

				projectile.Launch( Character, projectileDirection, typedInfo._projectileSpeed, typedInfo.BaseDamage, typedInfo.CanFriendlyFire );

				_behaviour.TryShoot();

				if ( _behaviour.IsReloading ) {

					break;
				}
			}

			Character.Pawn.SetTurretTarget( target.Pawn.transform );

			if ( typedInfo._sound != null ) {

				AudioSource.PlayClipAtPoint( typedInfo._sound, Character.Pawn.position, 0.5f );
			}
		}

		public override void Attack( Vector3 direction ) {

			if ( _behaviour.IsReloading ) {

				return;
			}

			if ( AttackCallback != null ) {

				AttackCallback();
			}

			for ( var i = 0; i < typedInfo._projectilesPerShot; ++i ) {

				var projectile = GetProjectileInstance();
				var projectileDirection = GetOffsetDirection( direction, i );

				projectile.Launch( Character, projectileDirection, typedInfo._projectileSpeed, typedInfo.BaseDamage, typedInfo.CanFriendlyFire );

				_behaviour.TryShoot();

				if ( _behaviour.IsReloading ) {

					break;
				}
			}

			if ( typedInfo._sound != null ) {

				AudioSource.PlayClipAtPoint( typedInfo._sound, Character.Pawn.position, 0.5f );
			}
		}

		public override bool CanAttack( Character target ) {

			return Vector3.Distance( target.Pawn.position, Character.Pawn.position ) <= typedInfo.AttackRange;
		}

		private Vector3 GetOffsetDirection( Vector3 direction, int index ) {

			var totalOffsetCount = typedInfo._projectilesPerShot;
			var coneAngle = typedInfo._shotConeAngle;

			if ( totalOffsetCount == 1 ) {

				return direction;
			}

			var normalizedOffsetIndex = (float) index / totalOffsetCount;
			var rotator = Quaternion.AngleAxis( Mathf.Lerp( -coneAngle, coneAngle, normalizedOffsetIndex ), Vector3.up );

			return rotator * direction;
		}

		private Projectile GetProjectileInstance() {

			var result = Instantiate( typedInfo._projectilePrefab );
			result.Lifetime = typedInfo._projectileLifetime;

			return result;
		}

	}

	public override Item GetItem() {

		return new RangedWeapon( this );
	}

	public override void Configure( Values values ) {

		base.Configure( values );

		ReloadDuration = BaseAttackSpeed;

		_projectileSpeed = values.Get( "Projectile Speed", 0f );
		_projectilesPerShot = values.Get( "BulletsPerBurst", 1 );
		_projectileLifetime = values.Get( "ProjectileLifetime", 1f );
		_shotConeAngle = values.Get( "BurstAngle", 0 );
		ClipSize = values.Get( "Clip Size", _projectilesPerShot );
		_projectilePrefab = values.GetPrefabWithComponent<Projectile>( "Projectile", fixName: false );
	}

}