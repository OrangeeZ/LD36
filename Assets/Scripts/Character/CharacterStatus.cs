using System;
using System.Collections.Generic;
using Expressions;
using UniRx;

[Serializable]
public class CharacterStatus {
	
	public IntReactiveProperty Strength;
	public IntReactiveProperty Agility;

	public IReactiveProperty<int> MaxHealth;
	public IReactiveProperty<float> MoveSpeed;

	private readonly CharacterStatusInfo _info;
	private List<CharacterStatusEffectInfo> _statusEffects = new List<CharacterStatusEffectInfo>(); 

	public CharacterStatus(CharacterStatusInfo info) {

		_info = info;

		Strength = new IntReactiveProperty( 0 );
		Agility = new IntReactiveProperty( 0 );

		MaxHealth = CreateCalculator( _info.HealthExpression ).Select( _ => (int)_ ).ToReactiveProperty();
		MoveSpeed = CreateCalculator( _info.MoveSpeedExpression ).Select( _ => (float)_ ).ToReactiveProperty();
	}

	private ReactiveCalculator CreateCalculator( IReactiveProperty<string> expression ) {

		var result = new ReactiveCalculator( expression );

		result.SubscribeProperty( "strength", Strength );
		result.SubscribeProperty( "agility", Agility );

		return result;
	}

	public void AddEffect( CharacterStatusEffectInfo statusEffect ) {
		
		statusEffect.Add( this );

		_statusEffects.Add( statusEffect );
    }

	public void RemoveEffect( CharacterStatusEffectInfo statusEffect ) {

		statusEffect.Remove( this );

		_statusEffects.Remove( statusEffect );
	}
}
