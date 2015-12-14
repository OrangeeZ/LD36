using System;
using UnityEngine;
using UniRx;
using System.Collections;
using Utility;

[CreateAssetMenu( menuName = "Create/States/Approach target" )]
public class ApproachTargetStateInfo : CharacterStateInfo {

	[Header( "Settings" )]
	[SerializeField]
	private float _minRange = 1.5f;

	[SerializeField]
	private float _maxRange = 4f;

	[SerializeField]
	private bool _autoActivate = true;

	[SerializeField]
	private bool _clearTargetOnReach = false;

	[Serializable]
	public class State : CharacterState<ApproachTargetStateInfo> {

		private TargetPosition destination;

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			stateController.character.InputSource.targets.Subscribe( SetDestination );
		}

		public override bool CanBeSet() {

			var distanceToDestination = destination.HasValue ? Vector3.Distance( character.Pawn.position, destination.Value ) : -1f;

			return destination.HasValue
			       && distanceToDestination >= typedInfo._minRange
			       && distanceToDestination <= typedInfo._maxRange;
		}

		public override IEnumerable GetEvaluationBlock() {

			var pawn = character.Pawn;

			pawn.canFollowDestination = true;
			pawn.SetDestination( destination.Value );

			do {

				yield return null;

			} while ( pawn.GetDistanceToDestination() > typedInfo._minRange && pawn.GetDistanceToDestination() < typedInfo._maxRange );

			pawn.ClearDestination();

			pawn.canFollowDestination = false;

			if ( typedInfo._clearTargetOnReach ) {

				destination = null;
			}
		}

		public void SetDestination( object target ) {

			if ( target is Vector3 ) {

				destination = (Vector3) target;
			} else if ( target is Character ) {

				destination = ( target as Character ).Pawn.transform;
			} else if ( target is ItemView ) {

				destination = ( target as ItemView ).transform;
			}

			if ( typedInfo._autoActivate ) {

				stateController.TrySetState( this );
			}
		}

		private void OnDestinationUpdate( Vector3 destination ) {
			Debug.Log( destination );
			this.destination = destination;

			stateController.TrySetState( this, allowEnterSameState: true );
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}