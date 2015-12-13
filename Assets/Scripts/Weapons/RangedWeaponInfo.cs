using UnityEngine;
using System.Collections;
using System.Xml.Schema;
using csv;
using Expressions;
using UniRx;

[CreateAssetMenu( menuName = "Create/Weapons/Ranged" )]
public class RangedWeaponInfo : WeaponInfo {

	//[CalculatorExpression]
	//[SerializeField]
	//private StringReactiveProperty _damageExpression;

	[SerializeField]
	private Projectile _projectilePrefab;

	[SerializeField]
	private float _projectileSpeed;

	[SerializeField]
	private int _clipSize;

	[SerializeField]
	private float _reloadDuration;

	[SerializeField]
	private int _projectilesPerShot;

	[SerializeField]
	private float _deviationCoefficient;

	[SerializeField]
	private float _shotConeAngle;

	[SerializeField]
	private AudioClip _sound;

	public class RangedWeapon : Weapon<RangedWeaponInfo> {

		public bool isReloading {
			get {

				if ( _isReloading && Time.timeSinceLevelLoad > _nextAttackTime ) {

					_isReloading = false;
				}

				return _isReloading;
			}
			private set { _isReloading = value; }
		}

		public int _ammoInClip { get; private set; }

		private float _nextAttackTime;
		//private readonly ReactiveCalculator _damageCalculator;
		private bool _isReloading;

		public RangedWeapon( RangedWeaponInfo info ) : base( info ) {

			_ammoInClip = info._clipSize;
			//_damageCalculator = new ReactiveCalculator( info._damageExpression );
		}

		public override void Attack( Character target, EnemyCharacterStatusInfo statusInfo ) {

			if ( target == null || Time.timeSinceLevelLoad < _nextAttackTime ) {

				return;
			}

			isReloading = false;

			if ( AttackCallback != null ) {

				AttackCallback();
			}

			for ( var i = 0; i < typedInfo._projectilesPerShot; ++i ) {

				var projectile = Instantiate( typedInfo._projectilePrefab );
				var targetDirection = ( target.Pawn.position - character.Pawn.position ).Set( y: 0 ).normalized;
				var projectileDirection = GetOffsetDirection( targetDirection, i );

				projectile.Launch( character, projectileDirection, typedInfo._projectileSpeed, typedInfo.BaseDamage, typedInfo.CanFriendlyFire );
				UpdateClipAndAttackTime();
			}

			character.Pawn.SetTurretTarget( target.Pawn.transform );

			if ( typedInfo._sound != null ) {

				AudioSource.PlayClipAtPoint( typedInfo._sound, character.Pawn.position, 0.5f );
			}
		}

		public override void Attack( Vector3 direction ) {

			if ( Time.timeSinceLevelLoad < _nextAttackTime ) {

				return;
			}

			isReloading = false;

			if ( AttackCallback != null ) {

				AttackCallback();
			}

			for ( var i = 0; i < typedInfo._projectilesPerShot; ++i ) {

                var projectile = Instantiate( typedInfo._projectilePrefab );
				var projectileDirection = GetOffsetDirection( direction, i );

				projectile.Launch( character, projectileDirection, typedInfo._projectileSpeed, typedInfo.BaseDamage, typedInfo.CanFriendlyFire );

				UpdateClipAndAttackTime();
			}
			//AudioSource.PlayClipAtPoint( typedInfo._sound, character.Pawn.position, 0.5f );
		}

		public override bool CanAttack( Character target ) {

			return Vector3.Distance( target.Pawn.position, character.Pawn.position ) <= typedInfo.AttackRange;
		}

		private void UpdateClipAndAttackTime() {

			_ammoInClip--;

			if ( _ammoInClip == 0 ) {

				_ammoInClip = typedInfo._clipSize;

				_nextAttackTime = Time.timeSinceLevelLoad + typedInfo._reloadDuration;

				isReloading = true;
			} else {

				_nextAttackTime = Time.timeSinceLevelLoad + typedInfo.BaseAttackSpeed;
			}
		}

		private Vector3 GetOffsetDirection( Vector3 direction, int index ) {

			var totalOffsetCount = typedInfo._projectilesPerShot;
			var coneAngle = typedInfo._shotConeAngle;

            if ( totalOffsetCount == 1 ) {

				return direction;
			}

			var normalizedOffsetIndex = (float)index / totalOffsetCount;
			var rotator = Quaternion.AngleAxis( Mathf.Lerp( -coneAngle, coneAngle, normalizedOffsetIndex ), Vector3.up );

			return rotator * direction;
		}

	}

	public override Item GetItem() {

		return new RangedWeapon( this );
	}

	public override void Configure( Values values ) {

		base.Configure( values );

		_reloadDuration = BaseAttackSpeed;

		_projectileSpeed = values.Get( "Projectile Speed", 0f );
		_projectilesPerShot = values.Get( "BulletsPerBurst", 1 );
		_shotConeAngle = values.Get( "BurstAngle", 0 );
		_clipSize = values.Get( "Clip Size", _projectilesPerShot );
		_projectilePrefab = values.GetPrefabWithComponent<Projectile>( "Projectile", fixName: false );
	}

}