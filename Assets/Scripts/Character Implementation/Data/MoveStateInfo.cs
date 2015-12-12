using CharacterFramework.Core;
using UnityEngine.ScriptableObjectWizard;

[Category( "CharacterBase states" )]
public class MoveStateInfo : CharacterStateInfo {

	public override CharacterStateBase GetState( Character character ) {

		return new MoveState( this, character );
	}

}