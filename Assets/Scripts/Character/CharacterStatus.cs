using System;
using System.Collections.Generic;
using Expressions;
using UniRx;

[Serializable]
public class CharacterStatus {

	public IntReactiveProperty Strength;
	public IntReactiveProperty Agility;

	public IReactiveProperty<float> MaxHealth;
	public IReactiveProperty<float> MoveSpeed;

	public readonly CharacterStatusInfo Info;

	private List<CharacterStatusEffectInfo> _statusEffects = new List<CharacterStatusEffectInfo>();

	public CharacterStatus( CharacterStatusInfo info ) {

		Info = info;

		Strength = new IntReactiveProperty( 0 );
		Agility = new IntReactiveProperty( 0 );

		MaxHealth = new ReactiveProperty<float>( Info.MaxHealth ); //CreateCalculator( _info.HealthExpression ).Select( _ => (int)_ ).ToReactiveProperty();
		MoveSpeed = new ReactiveProperty<float>( Info.MoveSpeed ); //CreateCalculator( _info.MoveSpeedExpression ).Select( _ => (float)_ ).ToReactiveProperty();
	}

	private ReactiveCalculator CreateCalculator( IReactiveProperty<string> expression ) {

		var result = new ReactiveCalculator( expression );

		result.SubscribeProperty( "strength", Strength );
		result.SubscribeProperty( "agility", Agility );

		return result;
	}

	public void AddEffect( CharacterStatusEffectInfo statusEffect ) {

		_statusEffects.Add( statusEffect );
	}

	public void RemoveEffect( CharacterStatusEffectInfo statusEffect ) {

		_statusEffects.Remove( statusEffect );
	}

}