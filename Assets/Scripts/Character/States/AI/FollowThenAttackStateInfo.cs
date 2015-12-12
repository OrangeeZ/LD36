using System.Linq;
using UniRx;
using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/States/Follow and attack" )]
public class FollowThenAttackStateInfo : CharacterStateInfo {

	public class State : CharacterState<FollowThenAttackStateInfo> {

		private FollowTargetStateInfo.State followState;

		private AttackStateInfo.State attackState;

		private CharacterPawnBase target;

		public State( CharacterStateInfo info )
			: base( info ) {
		}

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			followState = stateController.GetStateByInfo( typedInfo.followStateInfo ) as FollowTargetStateInfo.State;
			attackState = stateController.GetStateByInfo( typedInfo.attackStateInfo ) as AttackStateInfo.State;

			stateController.character.Pawn.GetSphereSensor().Subscribe( OnPawnClicked );
		}

		public override bool CanBeSet() {

			return target != null;
		}

		public override IEnumerable GetEvaluationBlock() {

			stateController.SetScheduledStates( new CharacterState[] { followState, attackState } );

			yield return null;
		}

		private void OnPawnClicked( CharacterPawnBase characterPawn ) {

			followState.SetTarget( characterPawn );
			attackState.SetTarget( characterPawn.character );

			stateController.TrySetState( this );
		}
	}

	[SerializeField]
	private CharacterStateInfo followStateInfo;

	[SerializeField]
	private CharacterStateInfo attackStateInfo;

	public override CharacterState GetState() {

		return new State( this );
	}
}
