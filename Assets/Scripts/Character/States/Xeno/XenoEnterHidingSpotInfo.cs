using UnityEngine;
using System.Collections;

public class XenoEnterHidingSpotInfo : CharacterStateInfo {

	private class State : CharacterState<XenoEnterHidingSpotInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override bool CanBeSet() {

			return base.CanBeSet();
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}