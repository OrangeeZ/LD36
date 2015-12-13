using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : AObject {

	[FormerlySerializedAs( "lifetime" )]
	public float Lifetime = 3f;

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

	private void Awake() {

		enabled = false;
	}

	protected virtual void Update() {

		if ( _timer.ValueNormalized >= 1f ) {

			OnLifetimeExpire();
		}

		position += Direction * Speed * Time.deltaTime;
	}

	public void Launch( Character owner, Vector3 direction, float speed, float damage, bool canFriendlyFire ) {

		this.Owner = owner;
		this.Speed = speed;
		this.Direction = direction;
		this.Damage = damage;
		this.CanFriendlyFire = canFriendlyFire;

		transform.position = this.Owner.Pawn.position;
		transform.rotation = this.Owner.Pawn.rotation;

		_timer = new AutoTimer( Lifetime );

		enabled = true;
	}

	public virtual void OnHit() {

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