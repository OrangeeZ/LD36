using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

[CreateAssetMenu( menuName = "Create/States/Xeno/Wait in hiding spot" )]
public class XenoWaitInHidingSpotInfo : CharacterStateInfo {

	private class State : CharacterState<XenoWaitInHidingSpotInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		private bool _isTriggered;

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

			while ( !_isTriggered ) yield return null;
		}

		private void OnXenoTriggerEvent( XenoTriggerEvent triggerEvent ) {

			_isTriggered = true;
		}
	}

	public override CharacterState GetState() {

		return new State( this );
	}

}