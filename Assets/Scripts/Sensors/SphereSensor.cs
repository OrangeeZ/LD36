using System;
using UniRx;
using UnityEngine;
using System.Collections;

public class SphereSensor : MonoBehaviour, IObservable<CharacterPawnBase> {

    private Subject<CharacterPawnBase> pawnSubject = new Subject<CharacterPawnBase>();

    public IDisposable Subscribe( IObserver<CharacterPawnBase> observer ) {

        return pawnSubject.Subscribe( observer );
    }

    private void OnTriggerEnter( Collider other ) {

        var otherPawn = other.gameObject.GetComponent<CharacterPawnBase>();

        if ( otherPawn != null ) {

            pawnSubject.OnNext( otherPawn );
        }
    }

}