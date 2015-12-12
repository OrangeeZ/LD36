using System.Collections;
using CharacterFramework.Core;
using Core.Traits;
using UnityEngine;

public class MoveState : CharacterStateBase {

	private readonly Character _character;

	public MoveState( CharacterStateInfoBase info, Character character ) : base( info ) {

		_character = character;
	}

	public override bool CanBeSet() {

		return GetMoveDirection().magnitude > 0;
	}

	public override IEnumerable GetEvaluationBlock() {

		while ( CanBeSet() ) {

			_character.Pawn.MoveDirection( GetMoveDirection() );

			yield return null;
		}
	}

	private Vector3 GetMoveDirection() {

		return Vector3.zero;//GameScreen.instance.moveJoystick.GetValue();
	}

}