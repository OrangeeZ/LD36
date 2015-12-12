using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterFramework.Core;

public abstract class CharacterStateBase {
    
    public readonly CharacterStateInfoBase info;

    protected CharacterBase character {
        get { return stateController.Character; }
    }

    protected HashSet<CharacterStateBase> possibleStates = new HashSet<CharacterStateBase>();

    protected CharacterStateController stateController;

    protected float deltaTime;

    protected CharacterStateBase( CharacterStateInfoBase info ) {

        this.info = info;
    }

    public void SetDeltaTime( float deltaTime ) {

        this.deltaTime = deltaTime;
    }

    public void SetTransitionStates( IEnumerable<CharacterStateBase> states ) {

        foreach ( var each in states ) {

            possibleStates.Add( each );
        }
    }

    public CharacterStateBase GetNextState() {

        return possibleStates.FirstOrDefault( which => which.CanBeSet() );
    }

    public virtual void Initialize( CharacterStateController stateController ) {

        this.stateController = stateController;
    }

    public virtual bool CanSwitchTo( CharacterStateBase nextState ) {

        return possibleStates.Contains( nextState );
    }

    public virtual bool CanBeSet() {

        return true;
    }

    public virtual IEnumerable GetEvaluationBlock() {

        yield return null;
    }

	public virtual void UpdateAnimator() {

		//stateController.CharacterBase.UpdateAnimation()//.Do( OnAnimationUpdate );
	}

	protected virtual void OnAnimationUpdate( CharacterComplexAnimationController animationController ) {
        
        animationController.SetBool( info.AnimatorStateName, true );
    }

}

//public class CharacterState<T> : CharacterState where T : CharacterStateInfo {

//    protected readonly T typedInfo;

//    public CharacterState( CharacterStateInfo info ) : base( info ) {

//        typedInfo = info as T;
//    }

//}