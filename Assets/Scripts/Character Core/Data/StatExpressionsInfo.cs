using Expressions;
using UniRx;
using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

[Category( "CharacterBase" )]
public class StatExpressionsInfo : ScriptableObject {

	[CalculatorExpression]
	public StringReactiveProperty healthExpression;

	[CalculatorExpression]
	public StringReactiveProperty moveSpeedExpression;
}
