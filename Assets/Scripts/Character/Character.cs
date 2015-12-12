using System;
using UniRx;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packages.EventSystem;

public class Character {

    public struct Died : IEventBase {

        public Character character;

    }

    public static List<Character> instances = new List<Character>();

    public readonly IntReactiveProperty health;

    public readonly IInputSource inputSource;

    public readonly IInventory inventory;

    public readonly CharacterPawn pawn;

    public readonly CharacterStateController stateController;
    public readonly CharacterStateController weaponStateController;

    public readonly int teamId;
    public readonly CharacterInfo info;

    public readonly CharacterStatus status;

    public ItemInfo itemToDrop;
    public float dropProbability = 0.15f;

    private readonly StatExpressionsInfo statExpressions;

    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

    public Character( StatExpressionsInfo statExpressions, CharacterPawn pawn, IInputSource inputSource, CharacterStatus status, CharacterStateController stateController, CharacterStateController weaponStateController, int teamId, CharacterInfo info ) {

        this.statExpressions = statExpressions;
        this.status = status;
        this.health = new IntReactiveProperty( this.status.maxHealth.Value );
        this.pawn = pawn;
        this.inputSource = inputSource;
        this.stateController = stateController;
        this.weaponStateController = weaponStateController;
        this.teamId = teamId;
        this.info = info;
        this.inventory = new BasicInventory( this );

        pawn.SetCharacter( this );

        this.stateController.Initialize( this );
        this.weaponStateController.Initialize( this );

        var inputSourceDisposable = inputSource as IDisposable;
        if ( inputSourceDisposable != null ) {

            _compositeDisposable.Add( inputSourceDisposable );
        }

        Observable.EveryUpdate().Subscribe( OnUpdate ).AddTo( _compositeDisposable );
        status.moveSpeed.Subscribe( UpdatePawnSpeed ).AddTo( _compositeDisposable );
        health.Subscribe( OnHealthChange );//.AddTo( _compositeDisposable );

        instances.Add( this );
    }

    private void OnHealthChange( int health ) {

        if ( health <= 0 ) {

            EventSystem.RaiseEvent( new Died { character = this } );

            instances.Remove( this );

            //_compositeDisposable.Dispose();
        }
    }

    private void OnUpdate( long ticks ) {

        stateController.Tick( Time.deltaTime );
        weaponStateController.Tick( Time.deltaTime );
    }

    public void Dispose() {

        _compositeDisposable.Dispose();
        health.Dispose();
    }

    private void UpdatePawnSpeed( float speed ) {

        pawn.SetSpeed( speed );
    }

}