using System;
using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityStandardAssets.Effects;

public class EffectSpawner : AObject {

	public GameObject CharacterDeathEffect;

	private void Start() {

		EventSystem.Events.SubscribeOfType<Character.Died>( OnCharacterDie );
	}

	private void OnCharacterDie( Character.Died diedEvent ) {

		Instantiate( CharacterDeathEffect, diedEvent.Character.Pawn.position, diedEvent.Character.Pawn.rotation );
	}

}