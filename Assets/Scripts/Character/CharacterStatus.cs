using System;
using Expressions;
using UniRx;

[Serializable]
public class CharacterStatus {
	
	public IntReactiveProperty Strength;
	public IntReactiveProperty Agility;

	public IReactiveProperty<int> MaxHealth;
	public IReactiveProperty<float> MoveSpeed;

	private readonly CharacterStatusInfo _info;

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
}
