using System.Collections;
using System.Collections.Generic;
using Packages.EventSystem;
using UniRx;
using UnityEngine;

[CreateAssetMenu( menuName = "Create/States/Xeno/Dead" )]
public class XenoDeadStateInfo : CharacterStateInfo {

	public struct Dead : IEventBase {

		public Character Character;

	}

	private class State : CharacterState<XenoDeadStateInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			character.Health.Where( _ => _ <= 0 ).Subscribe( _ => { stateController.ForceSetState( this ); } );
		}

		public override bool CanBeSet() {

			return character.Health.Value <= 0;
		}

		public override IEnumerable GetEvaluationBlock() {

			Debug.Log( "Dead" );

			EventSystem.RaiseEvent( new Dead {Character = character} );

			character.Pawn.ClearDestination();

			if ( stateController == character.StateController ) {

				character.Pawn.SetActive( false );

				var deathSound = character.Status.Info.DeathSounds.RandomElement();
				AudioSource.PlayClipAtPoint( deathSound, character.Pawn.position );

				if ( 1f.Random() <= character.dropProbability && !character.ItemsToDrop.IsNullOrEmpty() ) {

					character.ItemsToDrop.RandomElement().DropItem( character.Pawn.transform );
				}

				character.Pawn.MakeDead();
			}

			while ( CanBeSet() ) {

				yield return null;
			}
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}