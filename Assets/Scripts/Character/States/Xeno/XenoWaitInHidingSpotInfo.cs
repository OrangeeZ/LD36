using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packages.EventSystem;
using UniRx;
using UnityEditor;
using UnityEngine.UI;

[CreateAssetMenu( menuName = "Create/States/Xeno/Wait in hiding spot" )]
public class XenoWaitInHidingSpotInfo : CharacterStateInfo {

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
			var aggroCheckTimer = new AutoTimer( statusInfo.AutoAggroCheckInterval );

			while ( !_isTriggered ) {

				if ( aggroCheckTimer.ValueNormalized >= 1 ) {

					if ( CheckAutoAggro() ) {

						break;
					}

					aggroCheckTimer.Reset();
				}

				yield return null;
			}

			character.Pawn.SetActive( true );

			if ( _hidingSpot != null ) {

				_hidingSpot.TryResetState();
			}

			var scheduledStates = new List<CharacterState>();

			if ( statusInfo.IsAgressive ) {

				var approachState = character.StateController.GetState<ApproachTargetStateInfo.State>();
				approachState.SetDestination( _attackTarget );

				character.WeaponStateController.GetState<AttackStateInfo.State>().SetTarget( _attackTarget );

				scheduledStates.Add( approachState );
			}
			; // else 
			{

				var escapeState = stateController.GetState<XenoEscapeToVentilationHatchInfo.State>();
				escapeState.SetShouldSwitchRooms( true );
				scheduledStates.Add( escapeState );
			}

			stateController.SetScheduledStates( scheduledStates );
		}

		public void SetHidingSpot( EnvironmentObjectSpot hidingSpot ) {

			_hidingSpot = hidingSpot;
		}

		private void OnXenoTriggerEvent( XenoTriggerEvent triggerEvent ) {

			_isTriggered = true;
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

				OnXenoTriggerEvent( null );
			}
		}

		private bool CheckAutoAggro() {

			var statusInfo = character.Status.Info as EnemyCharacterStatusInfo;
			var randomChance = 1f.Random();
			Debug.Log( randomChance );

			return statusInfo.IsAgressive && statusInfo.AutoAggroChance >= randomChance;
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}