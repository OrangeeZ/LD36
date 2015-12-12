using UnityEngine;
using System.Collections;
using Expressions;
using UniRx;

[CreateAssetMenu( menuName = "Create/Status Info" )]
public class CharacterStatusInfo : ScriptableObject, ICsvConfigurable {
	
	[CalculatorExpression]
	public StringReactiveProperty HealthExpression;

	[CalculatorExpression]
	public StringReactiveProperty MoveSpeedExpression;

	[SerializeField]
	private float maxHP;
	[SerializeField]
	private float speed;
	[SerializeField]
	private float dmg;

	[SerializeField]
	private CharacterStatus status;

	public CharacterStatus GetInstance() {

		return new CharacterStatus( this ) {

			Agility = {Value = status.Agility.Value},
			Strength = {Value = status.Strength.Value}
		};
	}

	public void Configure (csv.Values values)
	{
		maxHP = values.Get("MaxHP", maxHP);
		speed = values.Get("Speed", maxHP);
		dmg = values.Get("DMG", maxHP);
	}
}