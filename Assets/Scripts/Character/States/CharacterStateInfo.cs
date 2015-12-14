using System;
using UnityEngine;

public abstract class CharacterStateInfo : ScriptableObject {

	[Header( "Animation settings" )]
	public string animatorStateName;
	public string weaponAnimatorStateName;

	public virtual CharacterState GetState() {

		throw new NotImplementedException();
	}
}
