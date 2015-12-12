using System;
using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

//[Category( "CharacterBase" )]

namespace CharacterFramework.Core {

	[HideInWizard]
	public abstract class CharacterInfo : ScriptableObject {

		[SerializeField]
		protected int TeamId = 0;
	}

}