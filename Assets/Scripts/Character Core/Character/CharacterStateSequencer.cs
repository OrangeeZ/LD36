using System.Linq;
using System.Collections;
using CharacterFramework.Core;

public class CharacterStateSequencer : CharacterStateBase {

	protected CharacterStateBase[] States;

	public CharacterStateSequencer( CharacterStateInfoBase info ) : base( info ) {
	}

	public override IEnumerable GetEvaluationBlock() {

		stateController.SetScheduledStates( States.Where( _ => _.CanBeSet() ) );

		yield return null;
	}
}
