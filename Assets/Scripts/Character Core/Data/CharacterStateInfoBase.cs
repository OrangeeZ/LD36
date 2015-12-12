using System;
using UnityEngine;
using UnityEngine.ScriptableObjectWizard;

namespace CharacterFramework.Core {

	[HideInWizard]
	public abstract class CharacterStateInfoBase : ScriptableObject {

		[Header( "Animation settings" )]
		public string AnimatorStateName;

	}

}