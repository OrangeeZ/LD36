using System.Collections;
using CharacterFramework.Core;
using Core.Traits;
using UniRx;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character states" )]
public class DeadStateInfo : CharacterStateInfo {

    private class State : CharacterStateBase {

	    private readonly Character _character;

        public State( DeadStateInfo info, Character character ) : base( info ) {

	        _character = character;
        }

        public override void Initialize( CharacterStateController stateController ) {

            base.Initialize( stateController );

            character.health.Where( _ => _ <= 0 ).Subscribe( _ => stateController.TrySetState( this ) );
        }

        public override bool CanBeSet() {

            return character.health.Value <= 0;
        }

        public override IEnumerable GetEvaluationBlock() {

	        var pawn = _character.Pawn;

			//pawn.ClearDestination();
			//pawn.SetGravityEnabled( true );
			//pawn.SetTurretTarget( null );

   //         GameplayController.instance.dangerLevel.Value += 1;

    //        if ( stateController == CharacterBase.stateController ) {

				//pawn.SetActive( false );

    //            //if ( 1f.Random() <= CharacterBase.dropProbability && CharacterBase.itemToDrop != null ) {

    //            //    CharacterBase.itemToDrop.DropItem( CharacterBase.pawn.transform );
    //            //}

				//pawn.MakeDead();
    //        }

            while ( CanBeSet() ) {

                yield return null;
            }
        }

    }

    public override CharacterStateBase GetState(Character character) {

        return new State( this, character );
    }

}