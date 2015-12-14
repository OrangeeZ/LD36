using csv;
using UnityEngine;

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
	private float _shotConeAngle;

	[SerializeField]
	private AudioClip[] _sounds;

	[SerializeField]
	private float _projectileLifetime;

	[SerializeField]
	private float _splashDamageRadius;

	[SerializeField]
	private RangedWeaponBehaviourInfo _weaponBehaviourInfo;

	[SerializeField]
	private BuffItemInfo _abilityOnPickup;

	public class RangedWeapon : Weapon<RangedWeaponInfo> {

		public int AmmoInClip { get; private set; }

		public float BaseAttackSpeed {
			get {
				return Character.Status.ModifierCalculator.CalculateFinalValue( ModifierType.BaseAttackSpeed, typedInfo.BaseAttackSpeed );
			}
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

			if ( typedInfo._abilityOnPickup != null ) {

				var buffItem = typedInfo._abilityOnPickup.GetItem();

				buffItem.SetCharacter( character );
				buffItem.Apply();
			}
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

				var finalDamage = Character.Status.ModifierCalculator.CalculateFinalValue( ModifierType.BaseDamage, typedInfo.BaseDamage );

				projectile.Launch( Character, projectileDirection, typedInfo._projectileSpeed, finalDamage, typedInfo.CanFriendlyFire, typedInfo._splashDamageRadius );

				_behaviour.TryShoot();

				if ( _behaviour.IsReloading ) {

					break;
				}
			}

			var sound = typedInfo._sounds.RandomElement();
			if ( sound != null ) {

				AudioSource.PlayClipAtPoint( sound, Character.Pawn.position, 0.5f );
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

				var finalDamage = Character.Status.ModifierCalculator.CalculateFinalValue( ModifierType.BaseDamage, typedInfo.BaseDamage );

				projectile.Launch( Character, projectileDirection, typedInfo._projectileSpeed, finalDamage, typedInfo.CanFriendlyFire, typedInfo._splashDamageRadius );

				_behaviour.TryShoot();

				if ( _behaviour.IsReloading ) {

					break;
				}
			}

			var sound = typedInfo._sounds.RandomElement();
            if ( sound != null ) {

				AudioSource.PlayClipAtPoint( sound, Character.Pawn.position, 0.5f );
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
		_splashDamageRadius = values.Get( "SplashRadius", float.NaN );
		_abilityOnPickup = values.GetScriptableObject<BuffItemInfo>( "AbilityOnPickup" );

		ClipSize = values.Get( "Clip Size", _projectilesPerShot );
		_projectilePrefab = values.GetPrefabWithComponent<Projectile>( "Projectile", fixName: false );
	}

}