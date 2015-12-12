using UnityEngine;

public class Projectile : AObject {

    public float lifetime = 3f;

    public int damage { get; protected set; }

    public float weight = 1f;

    private AutoTimer timer;

    protected Vector3 direction;

    protected Character owner;

    protected float speed;

    private void Awake() {

        enabled = false;
    }

    private void Update() {

        if ( timer.ValueNormalized >= 1f ) {

            OnLifetimeExpire();
        }

        position += direction * speed * Time.deltaTime;
    }

    public void Launch( Character owner, Vector3 direction, float speed, int damage ) {

        this.owner = owner;
        this.speed = speed;
        this.direction = direction;
        this.damage = damage;

        transform.position = this.owner.pawn.position;
        transform.rotation = this.owner.pawn.rotation;

        timer = new AutoTimer( lifetime );

        enabled = true;
    }

    public virtual void OnHit() {

        Destroy( gameObject );
    }

    public virtual void OnLifetimeExpire() {

        Destroy( gameObject );
    }

    private void OnTriggerEnter( Collider other ) {

        var otherPawn = other.GetComponent<CharacterPawn>();
        
        if ( otherPawn != null && otherPawn != owner.pawn && otherPawn.character != null && otherPawn.character.teamId != owner.teamId ) {

            otherPawn.character.health.Value -= 1;

            OnHit();

            return;
        }

        var otherBuilding = other.GetComponent<Building>();

        if ( otherBuilding != null ) {

            otherBuilding.Hit( this );

            OnHit();
        }
    }

}