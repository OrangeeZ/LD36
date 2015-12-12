using UnityEngine;
using System.Collections;
using Expressions;
using UniRx;

[CreateAssetMenu( menuName = "Create/Weapons/Ranged" )]
public class RangedWeaponInfo : WeaponInfo {

	[CalculatorExpression]
	[SerializeField]
	private StringReactiveProperty _damageExpression;

	[SerializeField]
	private Projectile _projectilePrefab;

	[SerializeField]
	private float _projectileSpeed;

	[SerializeField]
	private int _clipSize;

	[SerializeField]
	private float _reloadDuration;

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
		private readonly ReactiveCalculator _damageCalculator;
		private bool _isReloading;

		public RangedWeapon( RangedWeaponInfo info ) : base( info ) {

			_ammoInClip = info._clipSize;
			_damageCalculator = new ReactiveCalculator( info._damageExpression );
		}

		public override void Attack( Character target ) {

			if ( target == null || Time.timeSinceLevelLoad < _nextAttackTime ) {

				return;
			}

			isReloading = false;

			if ( AttackCallback != null ) {

				AttackCallback();
			}

			var projectile = Instantiate( typedInfo._projectilePrefab );
			var targetDirection = ( target.Pawn.position - character.Pawn.position ).Set( y: 0 ).normalized;

			projectile.Launch( character, targetDirection, typedInfo._projectileSpeed, (int) _damageCalculator.Result.Value );

			character.Pawn.SetTurretTarget( target.Pawn.transform );

			if ( typedInfo._sound != null ) {

				AudioSource.PlayClipAtPoint( typedInfo._sound, character.Pawn.position, 0.5f );
			}

			UpdateClipAndAttackTime();
		}

		public override void Attack( Vector3 direction ) {

			if ( Time.timeSinceLevelLoad < _nextAttackTime ) {

				return;
			}

			isReloading = false;

			if ( AttackCallback != null ) {

				AttackCallback();
			}

			var projectile = Instantiate( typedInfo._projectilePrefab );
			projectile.Launch( character, direction, typedInfo._projectileSpeed, (int) _damageCalculator.Result.Value );

			AudioSource.PlayClipAtPoint( typedInfo._sound, character.Pawn.position, 0.5f );

			UpdateClipAndAttackTime();
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

	}

	public override Item GetItem() {

		return new RangedWeapon( this );
	}

}