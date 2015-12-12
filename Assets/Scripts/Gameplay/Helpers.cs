using UnityEngine;
using System.Collections;
using System.Linq;
using Packages.EventSystem;

public static class Helpers {

    public struct SplashDamage : IEventBase {

        public Vector3 position;
        public float radius;

    }

    public static void DoSplashDamage( Vector3 point, float radius, int amount ) {

        new PMonad().Add( GradualDestroy( point, radius, amount ) ).Execute();

        EventSystem.RaiseEvent( new SplashDamage {position = point, radius = radius * 0.5f} );
    }

    private static IEnumerable GradualDestroy( Vector3 point, float radius, int amount ) {

        var maxObjectsPerIteration = 2;
        var objectCounter = 0;

        var affectedCharacters = Character.instances.Where( _ => ( _.pawn.position - point ).magnitude <= radius ).ToArray();
        foreach ( var each in affectedCharacters ) {

            each.health.Value -= amount;

            objectCounter++;

            if ( objectCounter >= maxObjectsPerIteration ) {

                objectCounter = 0;

                yield return null;
            }
        }

        var affectedBuildings = Building.instances.Where( _ => _.sphereCollider.Intersects( point, radius ) ).ToList();
        affectedBuildings.Sort( ( a, b ) => ( a.transform.position - point ).magnitude.CompareTo( ( b.transform.position - point ).magnitude ) );
        foreach ( var each in affectedBuildings ) {

            each.Hit( amount );

            objectCounter++;

            if ( objectCounter >= maxObjectsPerIteration ) {

                objectCounter = 0;

                yield return null;
            }
        }
    }

}