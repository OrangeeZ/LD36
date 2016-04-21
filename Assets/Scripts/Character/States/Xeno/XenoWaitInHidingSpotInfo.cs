using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Packages.EventSystem;
using UniRx;
using UnityEngine;

[CreateAssetMenu( menuName = "Create/States/Xeno/Wait in hiding spot" )]
public class XenoWaitInHidingSpotInfo : CharacterStateInfo {


    [SerializeField]
    private MapMarker _mapMarker;
    public class State : CharacterState<XenoWaitInHidingSpotInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		private bool _isTriggered;
		private EnvironmentObjectSpot _hidingSpot;
		private Character _attackTarget;

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			EventSystem.Events.SubscribeOfType<XenoTriggerEvent>( OnXenoTriggerEvent );
			EventSystem.Events.SubscribeOfType<RangedWeaponInfo.RangedWeapon.Fire>( OnWeaponFire );
		}

		public override bool CanSwitchTo( CharacterState nextState ) {

			return _isTriggered;
		}

		public override bool CanBeSet() {

			return !_isTriggered;
		}

		public override IEnumerable GetEvaluationBlock() {

			_isTriggered = false;

			var statusInfo = character.Status.Info as EnemyCharacterStatusInfo;

			if ( _hidingSpot != null ) {

				var pawn = character.Pawn as EnemyCharacterPawn;
                var marker = _hidingSpot.gameObject.GetComponent<MapMarker>();
                if (marker == null)
                {
                    marker = _hidingSpot.gameObject.AddComponent<MapMarker>();
                    //marker.markerSprite = typedInfo._mapMarker.markerSprite;
                    //marker.markerSize = typedInfo._mapMarker.markerSize;
                }
                marker.isActive = true;
                var fadeIn = pawn.Fade( isOut: false ).GetEnumerator();
				while ( fadeIn.MoveNext() ) {

					yield return null;
				}
                
                var aggroCheckTimer = new AutoTimer( statusInfo.AutoAggroCheckInterval );

				while ( !_isTriggered ) {

					pawn.SetPosition( _hidingSpot.position );

					if ( aggroCheckTimer.ValueNormalized >= 1 ) {

						if ( CheckAutoAggro() ) {

							break;
						}

						aggroCheckTimer.Reset();
					}

					yield return null;
				}
                marker.isActive = false;
                character.Pawn.SetActive( true );

				if ( _hidingSpot.GetState() == EnvironmentObjectSpot.State.Destroyed ) {

					character.Damage( 9999 );
				}

				var randomSound = character.Status.Info.IdleSounds.RandomElement();
				if ( randomSound != null ) {

					AudioSource.PlayClipAtPoint( randomSound, character.Pawn.position );
				}

				if ( _hidingSpot != null ) {

					_hidingSpot.TryResetState();
				}

				var fadeOut = pawn.Fade( isOut: true ).GetEnumerator();
				while ( fadeOut.MoveNext() ) {

					yield return null;
				}
			}

			var scheduledStates = new List<CharacterState>();

			if ( statusInfo.IsAgressive ) {

				var approachState = character.StateController.GetState<ApproachTargetStateInfo.State>();
				approachState.SetDestination( _attackTarget );

				character.WeaponStateController.GetState<AttackStateInfo.State>().SetTarget( _attackTarget );

				scheduledStates.Add( approachState );
			}

			var escapeState = stateController.GetState<XenoEscapeToVentilationHatchInfo.State>();
			escapeState.SetShouldSwitchRooms( true );
			scheduledStates.Add( escapeState );

			stateController.SetScheduledStates( scheduledStates );
		}

		public void SetHidingSpot( EnvironmentObjectSpot hidingSpot ) {

			_hidingSpot = hidingSpot;
		}

		private void OnXenoTriggerEvent( XenoTriggerEvent triggerEvent ) {

			var isInSameRoom = Room.FindRoomForPosition( triggerEvent.Source.position ) == Room.FindRoomForPosition( character.Pawn.position );

			_isTriggered = isInSameRoom;
		}

		private void OnWeaponFire( RangedWeaponInfo.RangedWeapon.Fire fireEvent ) {

			var thisPosition = character.Pawn.position;
			var otherPosition = fireEvent.Character.Pawn.position;
			var isInSameRoom = Room.FindRoomForPosition( otherPosition ) == Room.FindRoomForPosition( thisPosition );

			var statusInfo = character.Status.Info as EnemyCharacterStatusInfo;
			var isCloseEnough = Vector3.SqrMagnitude( thisPosition - otherPosition ) <= statusInfo.FrightenRadius.Pow( 2 );

			var isChanceEnough = 1f.Random() <= statusInfo.FrightenChance;

			_attackTarget = fireEvent.Character;

			if ( isChanceEnough && isCloseEnough && isInSameRoom ) {

				_isTriggered = true;
			}
		}

		private bool CheckAutoAggro() {

			var statusInfo = character.Status.Info as EnemyCharacterStatusInfo;
			var randomChance = 1f.Random();

			var playerCharacter = Character.Instances.First( _ => _.IsPlayerCharacter );
			var playerCharacterRoom = Room.FindRoomForPosition( playerCharacter.Pawn.position );

			var currentRoom = Room.FindRoomForPosition( character.Pawn.position );

			return statusInfo.IsAgressive && statusInfo.AutoAggroChance >= randomChance && currentRoom == playerCharacterRoom;
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}