using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

namespace AI.Gambits {

    [Category( "Gambits" )]
    public class Wander : GambitInfo {

        public float radius = 5f;

        public float interval = 1f;

        private class WanderGambit : Gambit<Wander> {

            private float lastExecutionTime = 0f;

            public WanderGambit( Character character, Wander info ) : base( character, info ) {
            }

            public override bool Execute() {

                if ( Time.time - lastExecutionTime < info.interval ) {

                    return false;
                }

                var point = character.pawn.position + character.pawn.rotation * Random.insideUnitCircle.normalized.ToXZ() * info.radius;
                
                character.stateController.GetState<ApproachTargetStateInfo.State>().SetDestination( point );

                lastExecutionTime = Time.time;

                return true;
            }

        }

        public override Gambit GetGambit( Character target ) {

            return new WanderGambit( target, this );
        }

    }

}