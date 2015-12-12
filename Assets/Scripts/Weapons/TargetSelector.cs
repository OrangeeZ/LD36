using System.Linq;
using CharacterFramework.Core;
using UnityEngine;

public static class TargetSelector {

    public static Character SelectTarget( Character currentCharacter, Vector3 direction ) {

	    return null;

	    //var planetTransform = currentCharacter.pawn.planetTransform;
	    //var characterToDirectionMap = Character.instances
	    //    .Where( _ => _ != currentCharacter )
	    //    .Where( _ => _.pawn.planetTransform.GetDistanceTo( currentCharacter.pawn.position ) < 15f )
	    //    .Select( _ => new {character = _, direction = Vector3.Dot( planetTransform.GetDirectionTo( _.pawn.position ), direction )} )
	    //    .Where( _ => _.direction >= 0.85f )
	    //    .ToList();

	    //characterToDirectionMap.Sort( ( a, b ) => ( b.character.pawn.position - currentCharacter.pawn.position ).magnitude.CompareTo( ( a.character.pawn.position - currentCharacter.pawn.position ).magnitude ) );
	    //characterToDirectionMap.Sort( ( a, b ) => a.direction.CompareTo( b.direction ) );

	    //return characterToDirectionMap.Any() ? characterToDirectionMap.First().character : null;
    }

}