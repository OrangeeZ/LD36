using UnityEngine;
using System.Collections;

public class ShrapnelProjectile : Projectile {

    public Projectile[] childProjectiles;

    public float maxDisplacement = 0.1f;

    public float damageScale = 1f;

    public override void OnHit() {

        base.OnHit();

        foreach ( var each in childProjectiles ) {

            var instance = Instantiate( each );

            instance.Launch( owner, direction + transform.rotation * Random.insideUnitCircle.normalized.ToXZ() * maxDisplacement, speed, ( damage * damageScale ).CeilToInt() );

            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
        }
    }

    public override void OnLifetimeExpire() {

        base.OnLifetimeExpire();

        OnHit();
    }

}