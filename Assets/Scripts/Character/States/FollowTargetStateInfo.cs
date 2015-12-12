using System;
using UniRx;
using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character states" )]
public class FollowTargetStateInfo : CharacterStateInfo {

	public float minDistance = 1f;

	public float maxDistance = 10f;

	public class State : CharacterState<FollowTargetStateInfo> {

		private CharacterPawnBase target;

		public State( CharacterStateInfo info )
			: base( info ) {
		}

		public override bool CanBeSet() {

			if ( target != null ) {

				var distance = ( character.pawn.position - target.position ).sqrMagnitude;

				return distance < typedInfo.maxDistance.Pow( 2 ) && distance > typedInfo.minDistance.Pow( 2 );
			}

			return false;
		}

		public override IEnumerable GetEvaluationBlock() {

		    yield return null;
		    //var pawn = character.pawn;
		    //var navAgent = pawn.GetNavMeshAgent();

		    //navAgent.destination = target.position;

		    //yield return null;

		    //while ( navAgent.pathPending ) {

		    //	yield return null;
		    //}

		    //navAgent.Resume();

		    //while ( true ) {

		    //	navAgent.destination = target.position;

		    //	if ( navAgent.remainingDistance > typedInfo.maxDistance ) {

		    //		target = null;

		    //		break;
		    //	}

		    //	if ( navAgent.remainingDistance < typedInfo.minDistance ) {

		    //		break;
		    //	}

		    //	yield return null;
		    //}

		    //navAgent.Stop();
		}

		public void SetTarget( CharacterPawnBase target ) {

			OnTargetUpdate( target );
		}

		private void OnTargetUpdate( CharacterPawnBase target ) {

			this.target = target;
			stateController.TrySetState( this );
		}
	}

	public override CharacterState GetState() {

		return new State( this );
	}
}
