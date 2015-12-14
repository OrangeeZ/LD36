using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using csv;

public enum ModifierType {

	None,

	ThornsDamage,

	SunHealthRestore,//
	ManureHealthRestore,//
	WaterHealthRestore,//

	BurningTimerDuration,
	DebuffTimerDuration,

	BaseAttackSpeed,//
	BaseRegeneration,
	BaseMoveSpeed,//
	BaseDamage//

}

public class ModifierCalculator {

	public Action Changed;

	private readonly Dictionary<ModifierType, List<OffsetValue>> _modifiers;

	public ModifierCalculator() {

		_modifiers = new Dictionary<ModifierType, List<OffsetValue>>();
	}

	public void Add( ModifierType modifierType, OffsetValue modifier ) {

		if ( !_modifiers.ContainsKey( modifierType ) ) {

			_modifiers[modifierType] = new List<OffsetValue>();
		}

		switch ( modifier.GetValueType() ) {

			case OffsetValue.OffsetValueType.Constant:
				_modifiers[modifierType].Insert( 0, modifier );
				break;

			case OffsetValue.OffsetValueType.Rate:
				_modifiers[modifierType].Add( modifier );
				break;
		}

		if ( Changed != null ) {

			Changed();
		}
	}

	public void Remove( ModifierType modifierType, OffsetValue modifier ) {

		if ( !_modifiers.ContainsKey( modifierType ) ) {

			return;
		}

		_modifiers[modifierType].Remove( modifier );

		if ( Changed != null ) {

			Changed();
		}
	}

	public float CalculateFinalValue( ModifierType modifierType, float baseValue ) {
		
		Debug.Log( "Before Calculate" );

		if ( modifierType == ModifierType.None || !_modifiers.ContainsKey( modifierType ) ) {

			return baseValue;
		}

		Debug.Log( "Before Before Calculate" );

		return _modifiers[modifierType].Aggregate( baseValue, ( total, each ) => each.Add( total ) );
	}

}

[CreateAssetMenu( menuName = "Create/Status effect info" )]
public class CharacterStatusEffectInfo : ScriptableObject {

	public int HealthDelta;
	public float MoveSpeedDelta;
	public int AmmoDelta;
	public float ViewRadiusDelta;

	

	public virtual void Add( Character target ) {

		target.Status.AddEffect( this );

		target.Status.MaxHealth.Value += HealthDelta;
		target.Status.MoveSpeed.Value += MoveSpeedDelta;

		
	}

	public virtual void Remove( Character target ) {

		target.Status.RemoveEffect( this );

		target.Status.MaxHealth.Value -= HealthDelta;
		target.Status.MoveSpeed.Value -= MoveSpeedDelta;

		//target.Status.ModifierCalculator.Remove( ModifierType, ModifierValue );
	}
}