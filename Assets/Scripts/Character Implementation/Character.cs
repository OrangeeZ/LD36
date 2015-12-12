using UnityEngine;
using System.Collections;
using CharacterFramework.Core;

public class Character : CharacterBase {

	public CharacterPawn Pawn;

	public Character( CharacterStatus status, int teamId ) : base( status, teamId ) {
	}

}
