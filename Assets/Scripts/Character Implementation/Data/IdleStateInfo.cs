using CharacterFramework.Core;
using UnityEngine.ScriptableObjectWizard;

[Category( "CharacterBase states" )]
public class IdleStateInfo : CharacterStateInfo {

	public override CharacterStateBase GetState( Character character ) {

		return new IdleState( this );
	}

}