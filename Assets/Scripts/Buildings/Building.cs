using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

    public static List<Building> instances = new List<Building>();

    public int health = 5;

    public SimpleSphereCollider sphereCollider;

    public EffectsBase[] effects;

    private void Awake() {

        instances.Add( this );
    }

    private void OnDestroy() {

        instances.Remove( this );
    }

    public virtual void Hit( Projectile projectile ) {

        Hit( projectile.damage );
    }

    public virtual void Hit( int damage ) {

        for ( var i = health - damage; i < health; i++ ) {

            if ( i >= 0 && i < effects.Length ) {

                effects[i].Activate();
            }
        }

        health -= damage;

        GameplayController.instance.dangerLevel.Value += 1;

        if ( health < 0 ) {

            Destroy();
        }
    }

    public virtual void Destroy() {

        GetComponent<Collider>().enabled = false;
    }

}