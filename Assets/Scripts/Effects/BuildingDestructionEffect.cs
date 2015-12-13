using UnityEngine;
using System.Collections;
using Packages.EventSystem;

public class BuildingDestructionEffect : EffectsBase {

    public Animation animation;

    public struct Destroyed : IEventBase {

        public Transform target;

    }

    public float delayBeforeDestruction = 0.5f;

    public override void Activate() {

        base.Activate();

        StartCoroutine( Loop() );
    }

    private IEnumerator Loop() {

        EventSystem.RaiseEvent( new Destroyed {target = transform} );

        yield return new WaitForSeconds( delayBeforeDestruction );

        if ( animation != null ) {

            animation.Play();
        } else {

            Destroy( gameObject );
        }
    }

}