using Expressions;
using UniRx;
using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/Stat Expressions Info" )]
public class StatExpressionsInfo : ScriptableObject {

	[CalculatorExpression]
	public StringReactiveProperty HealthExpression;

	[CalculatorExpression]
	public StringReactiveProperty MoveSpeedExpression;
}
