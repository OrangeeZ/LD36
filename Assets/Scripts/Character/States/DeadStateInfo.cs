using System.Collections;
using UniRx;
using UnityEngine;

[CreateAssetMenu( menuName = "Create/States/Dead" )]
public class DeadStateInfo : CharacterStateInfo {

    private class State : CharacterState<DeadStateInfo> {

        public State( CharacterStateInfo info ) : base( info ) {
        }

        public override void Initialize( CharacterStateController stateController ) {

            base.Initialize( stateController );

            character.health.Where( _ => _ <= 0 ).Subscribe( _ => stateController.TrySetState( this ) );
        }

        public override bool CanBeSet() {

            return character.health.Value <= 0;
        }

        public override IEnumerable GetEvaluationBlock() {

            character.pawn.ClearDestination();

            character.pawn.SetGravityEnabled( true );

            character.pawn.SetTurretTarget( null );

            GameplayController.instance.dangerLevel.Value += 1;

            if ( stateController == character.stateController ) {

                character.pawn.SetActive( false );

                if ( 1f.Random() <= character.dropProbability && character.itemToDrop != null ) {

                    character.itemToDrop.DropItem( character.pawn.transform );
                }

                character.pawn.MakeDead();
            }

            while ( CanBeSet() ) {

                yield return null;
            }
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}