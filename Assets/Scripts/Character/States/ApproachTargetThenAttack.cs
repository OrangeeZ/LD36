using System.Linq;
using UniRx;
using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character states" )]
public class ApproachTargetThenAttack : CharacterStateInfo {

	public class State : CharacterState {

		private ApproachTargetStateInfo.State approachState;

		private AttackStateInfo.State attackState;

		private CharacterPawnBase target;

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			this.approachState = stateController.states.OfType<ApproachTargetStateInfo.State>().First();
			this.attackState = stateController.states.OfType<AttackStateInfo.State>().First();

			stateController.character.inputSource.targets.OfType<object, Character>().Subscribe( OnCharacterClicked );
		}

		public override IEnumerable GetEvaluationBlock() {

			stateController.SetScheduledStates( new CharacterState[] { approachState, attackState } );

			yield return null;
		}

		private void OnCharacterClicked( Character character ) {

			approachState.SetDestination( character.pawn.position );
			attackState.SetTarget( character );

			stateController.TrySetState( this );
		}
	}

	//[SerializeField]
	//private CharacterStateInfo followStateInfo;

	//[SerializeField]
	//private CharacterStateInfo attackStateInfo;

	public override CharacterState GetState() {

		return new State( this );
	}
}
