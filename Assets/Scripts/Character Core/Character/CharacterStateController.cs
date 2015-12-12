using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using CharacterFramework.Core;

[System.Serializable]
public class CharacterStateController {

	public bool Debug = false;
    public bool UpdateAnimation = false;

    [HideInInspector]
	public CharacterBase Character;

	public IList<CharacterStateBase> States = null;

	private CharacterStateBase CurrentState { get; set; }

    private IEnumerator _evaluationBlock = null;

	private readonly Queue<CharacterStateBase> _scheduledStates = new Queue<CharacterStateBase>();

	public void Initialize( CharacterBase character ) {

		this.Character = character;

		foreach ( var each in States ) {

			each.Initialize( this );
		}
	}

	private void GetNextState() {

		var oldState = CurrentState;

		if ( _scheduledStates.Any() ) {

			CurrentState = _scheduledStates.Dequeue();
		} else if ( CurrentState != null ) {

			CurrentState = CurrentState.GetNextState();
		}

		if ( CurrentState == null ) {

			CurrentState = States.FirstOrDefault( that => that.CanBeSet() );
		}

		if ( oldState != CurrentState ) {

			LogTransition( oldState, CurrentState );
		}

		UpdateEvaluationBlock();
	}

	private void UpdateEvaluationBlock() {

		_evaluationBlock = new PMonad().Add( CurrentState.GetEvaluationBlock() ).Add( GetNextState ).ToEnumerator();
	}

	public void Tick( float deltaTime ) {

		if ( _evaluationBlock != null ) {

			CurrentState.SetDeltaTime( deltaTime );

			_evaluationBlock.MoveNext();

		    if ( UpdateAnimation ) {

				//if (Character is PlanetCharacter) (Character as PlanetCharacter).Pawn.UpdateAnimation( CurrentState );

				CurrentState.UpdateAnimator();
			}

		} else {

			GetNextState();
		}
	}

	public void TrySetState( CharacterStateBase newState, bool allowEnterSameState = false ) {

		if ( newState != CurrentState || !allowEnterSameState ) {

			if ( ( CurrentState != null && !CurrentState.CanSwitchTo( newState ) ) || !newState.CanBeSet() ) {

				return;
			}
		}

		LogTransition( CurrentState, newState );

		CurrentState = newState;

		UpdateEvaluationBlock();
	}

	public void TrySetState( CharacterStateInfoBase newStateInfo, bool allowEnterSameState = false ) {

		TrySetState( GetStateByInfo( newStateInfo ), allowEnterSameState );
	}

	public void SetScheduledStates( IEnumerable<CharacterStateBase> states ) {

		_scheduledStates.Clear();

		foreach ( var each in states ) {

			_scheduledStates.Enqueue( each );
		}
	}

	public CharacterStateBase GetStateByInfo( CharacterStateInfoBase info ) {

		return States.FirstOrDefault( where => where.info == info );
	}

	public T GetState<T>() where T : CharacterStateBase {

		return States.OfType<T>().FirstOrDefault();
	}

	private void LogTransition( CharacterStateBase from, CharacterStateBase to ) {
		
		if (!Debug) return;

		var fromName = from == null ? string.Empty : from.GetType().Name;
		var toName = to == null ? string.Empty : to.GetType().Name;

		UnityEngine.Debug.LogFormat( "{0}->{1}", fromName, toName );
	}

	//public void SetEvaluationBlock( IEnumerator evaluationBlock ) {

	//	this.evaluationBlock = evaluationBlock;
	//}
}
