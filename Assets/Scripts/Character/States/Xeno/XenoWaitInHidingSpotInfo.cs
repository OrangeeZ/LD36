using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

[CreateAssetMenu( menuName = "Create/States/Xeno/Wait in hiding spot" )]
public class XenoWaitInHidingSpotInfo : CharacterStateInfo {

	public class State : CharacterState<XenoWaitInHidingSpotInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		private bool _isTriggered;
		private EnvironmentObjectSpot _hidingSpot;

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			EventSystem.Events.SubscribeOfType<XenoTriggerEvent>( OnXenoTriggerEvent );
		}

		public override bool CanSwitchTo( CharacterState nextState ) {

			return _isTriggered;
		}

		public override bool CanBeSet() {

			return !_isTriggered;
		}

		public override IEnumerable GetEvaluationBlock() {

			_isTriggered = false;

			while ( !_isTriggered ) {
				yield return null;
			}

			character.Pawn.SetActive( true );

			if ( _hidingSpot != null ) {

				_hidingSpot.TryResetState();
			}
		}

		public void SetHidingSpot( EnvironmentObjectSpot hidingSpot ) {

			_hidingSpot = hidingSpot;
		}

		private void OnXenoTriggerEvent( XenoTriggerEvent triggerEvent ) {

			_isTriggered = true;
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}