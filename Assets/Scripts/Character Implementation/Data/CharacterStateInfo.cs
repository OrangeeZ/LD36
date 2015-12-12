using UnityEngine;
using System.Collections;
using CharacterFramework.Core;

public abstract class CharacterStateInfo : CharacterStateInfoBase {

	public abstract CharacterStateBase GetState( Character character );

}
