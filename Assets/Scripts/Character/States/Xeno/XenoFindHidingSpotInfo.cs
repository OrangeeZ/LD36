using UnityEngine;
using System.Collections;
using System.Linq;
using MoreLinq;

[CreateAssetMenu( menuName = "Create/States/Xeno/Find hiding spot" )]
public class XenoFindHidingSpotInfo : CharacterStateInfo {

	[SerializeField]
	private XenoWaitInHidingSpotInfo _waitInHidingSpotInfo;

	private class State : CharacterState<XenoFindHidingSpotInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override bool CanBeSet() {

			return GetFarthestObject() != null;
		}

		public override IEnumerable GetEvaluationBlock() {

			var pawn = character.Pawn as EnemyCharacterPawn;
			var hidingSpot = GetFarthestObject();

			do {

				yield return null;

				pawn.SetDestination( hidingSpot.transform.position );

				yield return null;

			} while ( pawn.GetDistanceToDestination() > 1f );

			pawn.SetPosition( hidingSpot.position );
			pawn.SetActive( false );
			
			hidingSpot.SetState( EnvironmentObjectSpot.State.Infected );

			var waitState = stateController.GetState<XenoWaitInHidingSpotInfo.State>();
			waitState.SetHidingSpot( hidingSpot );
			stateController.SetScheduledStates( new[] {waitState} );
		}

		private EnvironmentObjectSpot GetFarthestObject() {

			var position = character.Pawn.position;
			var room = Room.FindRoomForPosition( position );

			return room != null ? room.FindRandomObjectSpot( position ) : null;
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}