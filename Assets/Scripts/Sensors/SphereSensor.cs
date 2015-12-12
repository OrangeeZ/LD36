using System;
using UniRx;
using UnityEngine;
using System.Collections;

public class SphereSensor : MonoBehaviour, IObservable<CharacterPawnBase> {

    private readonly Subject<CharacterPawnBase> _pawnSubject = new Subject<CharacterPawnBase>();

    public IDisposable Subscribe( IObserver<CharacterPawnBase> observer ) {

        return _pawnSubject.Subscribe( observer );
    }

    private void OnTriggerEnter( Collider other ) {

        var otherPawn = other.transform.root.gameObject.GetComponent<CharacterPawnBase>();

        if ( otherPawn != null ) {

            _pawnSubject.OnNext( otherPawn );
        }
    }

}