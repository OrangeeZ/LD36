using System;
using UnityEngine;
using UnityEngine.ScriptableObjectWizard;

namespace AI.Gambits {

	[HideInWizard]
	[Category( "Gambits" )]
	public class GambitInfo : ScriptableObject {

		public virtual Gambit GetGambit( Character target ) {

			throw new NotImplementedException();
		}
	}
}