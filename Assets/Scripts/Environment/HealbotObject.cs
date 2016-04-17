using UnityEngine;
using System.Collections;

public class HealbotObject : EnvironmentObjectSpot {

	public override void Destroy( Character hittingCharacter ) {
		
		hittingCharacter.Heal( 999 );
	}

}
