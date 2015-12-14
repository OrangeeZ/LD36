using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : AObject {

	public float Lifetime;

	public float Damage { get; protected set; }

	public float LifeFraction {
		get { return _timer.ValueNormalized; }
	}

	public float weight = 1f;

	private AutoTimer _timer;

	protected Vector3 Direction;

	protected Character Owner;

	protected float Speed;
	protected bool CanFriendlyFire;
	private float _splashRange;

	private void Awake() {

		enabled = false;
	}

	protected virtual void Update() {

		if ( _timer.ValueNormalized >= 1f ) {

			OnLifetimeExpire();
		}

		position += Direction * Speed * Time.deltaTime;
	}

	public void Launch( Character owner, Vector3 direction, float speed, float damage, bool canFriendlyFire, float splashRange ) {

		this.Owner = owner;
		this.Speed = speed;
		this.Direction = direction;
		this.Damage = damage;
		this.CanFriendlyFire = canFriendlyFire;
		_splashRange = splashRange;

		transform.position = this.Owner.Pawn.GetWeaponPosition();
		transform.rotation = this.Owner.Pawn.rotation;

		_timer = new AutoTimer( Lifetime );

		enabled = true;
	}

	public virtual void OnHit() {

		if ( !_splashRange.IsNan() && _splashRange > 0f ) {

			Helpers.DoSplashDamage( transform.position, _splashRange, Damage, teamToSkip: CanFriendlyFire ? -1 : Owner.TeamId );
		}

		Release();
	}

	public virtual void OnContact( Collider other ) {

	}

	public virtual void OnLifetimeExpire() {

		Release();
	}

	protected virtual void Release() {

		Destroy( gameObject );
	}

	private void OnTriggerEnter( Collider other ) {

		var otherPawn = other.GetComponent<CharacterPawnBase>();

		if ( otherPawn != null && otherPawn != Owner.Pawn && otherPawn.character != null ) {

			var canAttackTarget = !CanFriendlyFire || otherPawn.character.TeamId != Owner.TeamId;

			if ( canAttackTarget ) {

				otherPawn.character.Damage( Damage );

				OnContact( other );
				OnHit();
			}

			return;
		}

		OnContact( other );

		var otherBuilding = other.GetComponent<Building>();

		if ( otherBuilding != null ) {

			otherBuilding.Hit( this );
			OnHit();
		}
	}

}