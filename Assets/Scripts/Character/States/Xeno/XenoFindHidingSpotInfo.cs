using UnityEngine;
using System.Collections;
using MoreLinq;

[CreateAssetMenu( menuName = "Create/States/Xeno/Find hiding spot" )]
public class XenoFindHidingSpotInfo : CharacterStateInfo {

	[SerializeField]
	private XenoWaitInHidingSpotInfo _waitInHidingSpotInfo;

	private class State : CharacterState<XenoFindHidingSpotInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override bool CanBeSet() {

			return GetFathestObject() != null;
		}

		public override IEnumerable GetEvaluationBlock() {

			var pawn = character.Pawn;
			var hidingSpot = GetFathestObject();

			do {

				yield return null;

				pawn.SetDestination( hidingSpot.transform.position );

				yield return null;

			} while ( pawn.GetDistanceToDestination() > 1f );

			character.Pawn.SetActive( false );
			hidingSpot.SetState( EnvironmentObject.State.Infected );

			var waitState = stateController.GetStateByInfo( typedInfo._waitInHidingSpotInfo );
			stateController.SetScheduledStates( new[] {waitState} );
		}

		private EnvironmentObject GetFathestObject() {

			return EnvironmentObject.Instances.MaxBy( each => Vector3.SqrMagnitude( each.transform.position - character.Pawn.position ) );
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}