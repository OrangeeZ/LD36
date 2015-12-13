using System;
using UniRx;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packages.EventSystem;

public class Character {

	public struct Died : IEventBase {

		public Character Character;

	}

	public struct RecievedDamage : IEventBase {

		public Character Character;
		public float Damage;

	}

	public static List<Character> Instances = new List<Character>();

	public readonly FloatReactiveProperty Health;

	public readonly IInputSource InputSource;

	public readonly IInventory Inventory;

	public readonly CharacterPawn Pawn;

	public readonly CharacterStateController StateController;
	public readonly CharacterStateController WeaponStateController;

	public readonly int TeamId;
	public readonly CharacterInfo Info;

	public readonly CharacterStatus Status;

	public ItemInfo[] ItemsToDrop;
	public float dropProbability = 0.15f;

	private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

	public Character( CharacterPawn pawn, IInputSource inputSource, CharacterStatus status, CharacterStateController stateController, CharacterStateController weaponStateController, int teamId, CharacterInfo info ) {

		this.Status = status;
		this.Health = new FloatReactiveProperty( this.Status.MaxHealth.Value );
		this.Pawn = pawn;
		this.InputSource = inputSource;
		this.StateController = stateController;
		this.WeaponStateController = weaponStateController;
		this.TeamId = teamId;
		this.Info = info;
		this.Inventory = new BasicInventory( this );

		pawn.SetCharacter( this );

		this.StateController.Initialize( this );
		this.WeaponStateController.Initialize( this );

		var inputSourceDisposable = inputSource as IDisposable;
		if ( inputSourceDisposable != null ) {

			_compositeDisposable.Add( inputSourceDisposable );
		}

		Observable.EveryUpdate().Subscribe( OnUpdate ).AddTo( _compositeDisposable );
		status.MoveSpeed.Subscribe( UpdatePawnSpeed ).AddTo( _compositeDisposable );
		Health.Subscribe( OnHealthChange ); //.AddTo( _compositeDisposable );

		Instances.Add( this );

		Status.ModifierCalculator.Changed += OnModifiersChange;
	}

	private void OnHealthChange( float health ) {

		if ( health <= 0 ) {

			EventSystem.RaiseEvent( new Died {Character = this} );

			Instances.Remove( this );

			//_compositeDisposable.Dispose();

			Status.ModifierCalculator.Changed -= OnModifiersChange;
		}
	}

	public void Damage( float amount ) {

		if ( Health.Value <= 0 ) {

			return;
		}

		Health.Value -= amount;

		EventSystem.RaiseEvent( new RecievedDamage {Character = this, Damage = amount} );
	}

	public void Dispose() {

		_compositeDisposable.Dispose();
		Health.Dispose();
	}

	private void OnUpdate( long ticks ) {

		StateController.Tick( Time.deltaTime );
		WeaponStateController.Tick( Time.deltaTime );
	}

	private void UpdatePawnSpeed( float speed ) {

		Pawn.SetSpeed( speed );
	}

	private void OnModifiersChange() {

	}
}