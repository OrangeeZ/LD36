using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityStandardAssets.Effects;

public class EffectSpawner : AObject {

    public GameObject explosion;
    public GameObject splashExplosion;
    public GameObject buildingExplosion;

    private void Start() {

        EventSystem.Events.SubscribeOfType<Character.Died>( OnCharacterDie );
        EventSystem.Events.SubscribeOfType<Helpers.SplashDamage>( OnSplashDamage );

        EventSystem.Events.SubscribeOfType<BuildingDestructionEffect.Destroyed>( OnBuildingDestruction );
    }

    private void OnBuildingDestruction( BuildingDestructionEffect.Destroyed buildingDestroyedEvent ) {

        Instantiate( buildingExplosion, buildingDestroyedEvent.target.position, buildingDestroyedEvent.target.rotation );
    }

    private void OnSplashDamage( Helpers.SplashDamage splashDamageEvent ) {

        var instance = Instantiate( splashExplosion, splashDamageEvent.position, Quaternion.identity ) as GameObject;
        var scaler = instance.GetComponent<ParticleSystemMultiplier>();

        if ( scaler != null ) {

            scaler.multiplier = splashDamageEvent.radius;
        }
    }

    private void OnCharacterDie( Character.Died diedEvent ) {

        Instantiate( explosion, diedEvent.Character.Pawn.position, diedEvent.Character.Pawn.rotation );
    }

}