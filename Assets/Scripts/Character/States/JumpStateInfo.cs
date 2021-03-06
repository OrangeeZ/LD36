﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/States/Jump" )]
public class JumpStateInfo : CharacterStateInfo {

	[SerializeField]
	private float _jumpImpulse = 5f;

	private class State : CharacterState<JumpStateInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override bool CanBeSet() {

			return character.Pawn.IsGrounded() && Input.GetButton( "Jump" );
		}

		public override IEnumerable GetEvaluationBlock() {

			var impulse = typedInfo._jumpImpulse;

			while ( impulse > 0 ) {

				impulse += Physics.gravity.y * deltaTime;

				character.Pawn.MoveHorizontal( GetMoveDirection() );
				character.Pawn.MoveVertical( ref impulse, deltaTime );

				yield return null;
			}
		}

		private Vector3 GetMoveDirection() {

			return new Vector3( Input.GetAxis( "Horizontal" ), 0, 0 ).ClampMagnitude( 1f ); //GameScreen.instance.moveJoystick.GetValue();
			return new Vector3( Input.GetAxis( "Horizontal" ), 0, Input.GetAxis( "Vertical" ) ).ClampMagnitude( 1f ); //GameScreen.instance.moveJoystick.GetValue();
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}