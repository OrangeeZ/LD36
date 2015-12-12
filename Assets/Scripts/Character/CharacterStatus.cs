using System;
using Expressions;
using UniRx;

[Serializable]
public class CharacterStatus {

	public IntReactiveProperty strength;
	public IntReactiveProperty agility;

	public IReactiveProperty<int> maxHealth;
	public IReactiveProperty<float> moveSpeed;

	public CharacterStatus( StatExpressionsInfo expressionsInfo ) {

		strength = new IntReactiveProperty( 0 );
		agility = new IntReactiveProperty( 0 );

		maxHealth = CreateCalculator( expressionsInfo.HealthExpression ).Select( _ => (int)_ ).ToReactiveProperty();
		moveSpeed = CreateCalculator( expressionsInfo.MoveSpeedExpression ).Select( _ => (float)_ ).ToReactiveProperty();
	}

	private ReactiveCalculator CreateCalculator( IReactiveProperty<string> expression ) {

		var result = new ReactiveCalculator( expression );

		result.SubscribeProperty( "strength", strength );
		result.SubscribeProperty( "agility", agility );

		return result;
	}
}
