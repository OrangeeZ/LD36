using UnityEngine;

public class Projectile : AObject {

    public float lifetime = 3f;

    public float damage { get; protected set; }

    public float weight = 1f;

    private AutoTimer _timer;

    protected Vector3 Direction;

    protected Character Owner;

    protected float Speed;
	protected bool CanFriendlyFire;

	private void Awake() {

        enabled = false;
    }

    private void Update() {

        if ( _timer.ValueNormalized >= 1f ) {

            OnLifetimeExpire();
        }

        position += Direction * Speed * Time.deltaTime;
    }

    public void Launch( Character owner, Vector3 direction, float speed, float damage, bool canFriendlyFire ) {

        this.Owner = owner;
        this.Speed = speed;
        this.Direction = direction;
        this.damage = damage;
	    this.CanFriendlyFire = canFriendlyFire;

        transform.position = this.Owner.Pawn.position;
        transform.rotation = this.Owner.Pawn.rotation;

        _timer = new AutoTimer( lifetime );

        enabled = true;
    }

    public virtual void OnHit() {

        Destroy( gameObject );
    }

    public virtual void OnLifetimeExpire() {

        Destroy( gameObject );
    }

    private void OnTriggerEnter( Collider other ) {

        var otherPawn = other.GetComponent<CharacterPawnBase>();
        
        if ( otherPawn != null && otherPawn != Owner.Pawn && otherPawn.character != null ) {

	        var canAttackTarget = !CanFriendlyFire || otherPawn.character.TeamId != Owner.TeamId;

	        if ( canAttackTarget ) {
		        otherPawn.character.Health.Value -= 1;

		        OnHit();
	        }

	        return;
        }

        var otherBuilding = other.GetComponent<Building>();

        if ( otherBuilding != null ) {

            otherBuilding.Hit( this );

            OnHit();
        }
    }

}