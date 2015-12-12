using UnityEngine;
using System.Collections;
using CharacterFramework.Core;
using Expressions;
using UniRx;
using UnityEngine.ScriptableObjectWizard;

[Category( "Weapons" )]
public class RangedWeaponInfo : WeaponInfo {

    [CalculatorExpression]
    [SerializeField]
    private StringReactiveProperty _damageExpression;

    //[SerializeField]
    //private PlanetProjectile _planetProjectilePrefab;

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
            //_damageCalculator.SubscribeProperty( "dangerLevel", GameplayController.instance.dangerLevel );
        }

        public override void Attack( Character target ) {

            if ( target == null || Time.timeSinceLevelLoad < _nextAttackTime ) {

                return;
            }

            isReloading = false;

            if ( attackCallback != null ) {

                attackCallback();
            }

            //var projectile = Instantiate( typedInfo._planetProjectilePrefab );
            //var targetDirection = character.pawn.planetTransform.GetDirectionTo( target.pawn.planetTransform );

            //targetDirection.y *= 1.2f;

            //PlanetProjectile.Launch( character, targetDirection, typedInfo._projectileSpeed, (int) _damageCalculator.Result.Value );

            //character.pawn.SetTurretTarget( target.pawn.transform );

            //AudioSource.PlayClipAtPoint( typedInfo._sound, character.pawn.position, 0.5f );

            UpdateClipAndAttackTime();
        }

        public override void Attack( Vector3 direction ) {

            if ( Time.timeSinceLevelLoad < _nextAttackTime ) {

                return;
            }

            isReloading = false;

            if ( attackCallback != null ) {

                attackCallback();
            }

            //var projectile = Instantiate( typedInfo._planetProjectilePrefab );
            //projectile.Launch( character as PlanetCharacter, direction, typedInfo._projectileSpeed, (int) _damageCalculator.Result.Value );

            //AudioSource.PlayClipAtPoint( typedInfo._sound, character.pawn.position, 0.5f );

            UpdateClipAndAttackTime();
        }

        public override bool CanAttack( Character target ) {

	        return false;
	        //return target.pawn.planetTransform.GetDistanceTo( character.pawn.position ) <= typedInfo.attackRange;
        }

        private void UpdateClipAndAttackTime() {

            _ammoInClip--;

            if ( _ammoInClip == 0 ) {

                _ammoInClip = typedInfo._clipSize;

                _nextAttackTime = Time.timeSinceLevelLoad + typedInfo._reloadDuration;

                isReloading = true;
            } else {

                _nextAttackTime = Time.timeSinceLevelLoad + typedInfo.baseAttackSpeed;
            }
        }

    }

    public override Item GetItem() {

        return new RangedWeapon( this );
    }

}