using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using UnityEngine.ScriptableObjectWizard;

namespace CharacterFramework.Core {

	[Serializable]
	public class StateTransitionEntry<TStateInfo> where TStateInfo : CharacterStateInfoBase {

		public TStateInfo StateInfo;
		public bool[] TransitionMask;

	}

	[Category( "CharacterBase" )]
	public class CharacterStateControllerInfo<TStateTransitionEntry, TStateInfo, TCharacterState, TCharacter> : ScriptableObject where TStateTransitionEntry : StateTransitionEntry<TStateInfo>
		where TStateInfo : CharacterStateInfoBase
		where TCharacterState : CharacterStateBase
		where TCharacter : CharacterBase {

		[SerializeField]
		private bool _updateAnimation = false;

		[SerializeField]
		private bool _isDebug = false;

		[SerializeField]
		protected List<TStateTransitionEntry> Entries;

		protected virtual IEnumerable<TCharacterState> GetStates( TCharacter character ) {

			yield break;
			//return _entries.Select( _=>_.StateInfo. )
		}

		public CharacterStateController GetStateController( TCharacter character ) {

			var result = new CharacterStateController {

				Debug = _isDebug,
				UpdateAnimation = _updateAnimation,
				Character = character,
				States = GetStates( character ).ToArray(),
			};

			foreach ( var each in result.States ) {

				var transitionEntry = Entries[result.States.IndexOf( each )];

				each.SetTransitionStates( result.States
					.EquiZip( transitionEntry.TransitionMask, ( a, b ) => new {item1 = a, item2 = b} )
					.Where( _ => _.item2 && _.item1 != each )
					.Select( _ => _.item1 ) );
			}

			return result;
		}

	}

}