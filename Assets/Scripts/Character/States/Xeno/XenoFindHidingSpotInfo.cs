using UnityEngine;
using System.Collections;
using MoreLinq;

[CreateAssetMenu( menuName = "Create/States/Xeno/Find hiding spot" )]
public class XenoFindHidingSpotInfo : CharacterStateInfo {

	private class State : CharacterState<XenoFindHidingSpotInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override bool CanBeSet() {

			return GetFathestObject() != null;
		}

		public override IEnumerable GetEvaluationBlock() {

			var pawn = character.Pawn;
			var farthestObject = GetFathestObject();

			do {

				yield return null;

				pawn.SetDestination( farthestObject.transform.position );

				yield return null;

			} while ( pawn.GetDistanceToDestination() > 1f );
		}

		private EnvironmentObject GetFathestObject() {

			return EnvironmentObject.Instances.MaxBy( each => Vector3.SqrMagnitude( each.transform.position - character.Pawn.position ) );
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}