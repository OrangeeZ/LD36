using UnityEngine;
using System.Collections;
using Expressions;
using UniRx;

[CreateAssetMenu( menuName = "Create/Status Info" )]
public class CharacterStatusInfo : ScriptableObject {
	
	[CalculatorExpression]
	public StringReactiveProperty HealthExpression;

	[CalculatorExpression]
	public StringReactiveProperty MoveSpeedExpression;

	[SerializeField]
	private CharacterStatus status;

	public CharacterStatus GetInstance() {

		return new CharacterStatus( this ) {

			Agility = {Value = status.Agility.Value},
			Strength = {Value = status.Strength.Value}
		};
	}
}