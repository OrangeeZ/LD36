using UnityEngine;
using System.Collections;
using System.Linq;
using MoreLinq;

[CreateAssetMenu( menuName = "Create/States/Xeno/Escape to ventilation hatch" )]
public class XenoEscapeToVentilationHatchInfo : CharacterStateInfo {

	public class State : CharacterState<XenoEscapeToVentilationHatchInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		private bool _shouldSwitchRooms = false;

		public override bool CanBeSet() {

			return _shouldSwitchRooms;
		}

		public override IEnumerable GetEvaluationBlock() {

			_shouldSwitchRooms = false;

			var pawn = character.Pawn as EnemyCharacterPawn;
			var currentRoom = Room.FindRoomForPosition( pawn.position );
			var closestVentilationHatch = currentRoom.FindClosestVentilationHatchPosition( pawn.position );

			do {

				yield return null;

				pawn.SetDestination( closestVentilationHatch );

				yield return null;

			} while ( pawn.GetDistanceToDestination() > 1f );

			var fade = pawn.Fade( isOut: false ).GetEnumerator();
			while ( fade.MoveNext() ) {

				yield return null;
			}

			var randomRoom = Room.RandomRoomExcept( currentRoom );
			pawn.SetPosition( randomRoom.transform.position );
		}

		public void SetShouldSwitchRooms( bool value ) {

			_shouldSwitchRooms = value;
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}