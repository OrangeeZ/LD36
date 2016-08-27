using UnityEngine;
using System.Collections;
using UniRx;

public class InteractWithDeviceStateInfo : CharacterStateInfo {

	public float duration = .5f;

	private class State : CharacterState<InteractWithDeviceStateInfo> {

		public State( CharacterStateInfo info )
			: base( info ) {

			Observable.EveryUpdate().Subscribe( CheckInput );
		}

		public override bool CanBeSet() {

			return character.Pawn.RoomDeviceListener.RoomDevice != null;
		}

		public override IEnumerable GetEvaluationBlock() {

			var timer = new AutoTimer( typedInfo.duration );
			var device = character.Pawn.RoomDeviceListener.RoomDevice;

			while ( timer.ValueNormalized < 1 ) {

				yield return null;
			}

			device.SetFixed();
			device.Interact();
		}

		private void CheckInput( long ticks ) {

			if ( Input.GetButton( "Interact" ) ) {
				
				stateController.TrySetState( this );
			}
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}