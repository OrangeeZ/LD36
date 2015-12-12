using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/States/Move" )]
public class MoveStateInfo : CharacterStateInfo {

	private class State : CharacterState<MoveStateInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override bool CanBeSet() {

			return GetMoveDirection().magnitude > 0;
		}

		public override IEnumerable GetEvaluationBlock() {

			while ( CanBeSet() ) {

				character.pawn.MoveDirection( GetMoveDirection() );

				yield return null;
			}
		}

		private Vector3 GetMoveDirection() {

			return new Vector3( Input.GetAxis( "Horizontal" ), 0, Input.GetAxis( "Vertical" ) ).normalized; //GameScreen.instance.moveJoystick.GetValue();
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}