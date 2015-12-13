using System;
using System.Collections.Generic;
using System.Linq;
using Expressions;
using UniRx;

[Serializable]
public class CharacterStatus {

	public IntReactiveProperty Strength;
	public IntReactiveProperty Agility;

	public IReactiveProperty<float> MaxHealth;
	public IReactiveProperty<float> MoveSpeed;

	public readonly CharacterStatusInfo Info;

	public readonly ModifierCalculator ModifierCalculator;

	private List<CharacterStatusEffectInfo> _statusEffects = new List<CharacterStatusEffectInfo>();

	public CharacterStatus( CharacterStatusInfo info ) {

		Info = info;

		Strength = new IntReactiveProperty( 0 );
		Agility = new IntReactiveProperty( 0 );

		MaxHealth = new ReactiveProperty<float>( Info.MaxHealth );
		MoveSpeed = new ReactiveProperty<float>( Info.MoveSpeed );

		ModifierCalculator = new ModifierCalculator();
    }

	public void AddEffect( CharacterStatusEffectInfo statusEffect ) {

		_statusEffects.Add( statusEffect );
	}

	public void RemoveEffect( CharacterStatusEffectInfo statusEffect ) {

		_statusEffects.Remove( statusEffect );
	}
}