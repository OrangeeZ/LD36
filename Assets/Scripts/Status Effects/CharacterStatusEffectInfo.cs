using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using csv;

public enum ModifierType {

	None,

	ThornsDamage,

	SunHealthRestore,
	ManureHealthRestore,
	WaterHealthRestore,

	BurningTimerDuration,
	DebuffTimerDuration,

	BaseAttackSpeed,
	BaseRegeneration,
	BaseMoveSpeed,
	BaseDamage

}

public class ModifierCalculator {

	private readonly Dictionary<ModifierType, List<OffsetValue>> _modifiers = new Dictionary<ModifierType, List<OffsetValue>>();

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
	}

	public void Remove( ModifierType modifierType, OffsetValue modifier ) {

		_modifiers[modifierType].Remove( modifier );
	}

	public float CalculateFinalValue( ModifierType modifierType, float baseValue ) {

		if ( modifierType == ModifierType.None ) {

			return baseValue;
		}

		return _modifiers[modifierType].Aggregate( baseValue, ( total, each ) => each.Add( total ) );
	}

}

[CreateAssetMenu( menuName = "Create/Status effect info" )]
public class CharacterStatusEffectInfo : ScriptableObject, ICsvConfigurable {

	public int HealthDelta;
	public float MoveSpeedDelta;
	public int AmmoDelta;
	public float ViewRadiusDelta;

	public ModifierType ModifierType;
	public OffsetValue ModifierValue;

	public virtual void Add( Character target ) {

		target.Status.AddEffect( this );

		target.Status.MaxHealth.Value += HealthDelta;
		target.Status.MoveSpeed.Value += MoveSpeedDelta;

		target.Status.ModifierCalculator.Add( ModifierType, ModifierValue );
	}

	public virtual void Remove( Character target ) {

		target.Status.RemoveEffect( this );

		target.Status.MaxHealth.Value -= HealthDelta;
		target.Status.MoveSpeed.Value -= MoveSpeedDelta;

		target.Status.ModifierCalculator.Remove( ModifierType, ModifierValue );
	}

	public void Configure( Values values ) {

		ModifierType = ParseModifierType( values.Get( "AffectedStat", string.Empty ) );
		ModifierValue = new OffsetValue( values.Get( "Amount", -1f ), ParseOffsetValueType( values.Get( "ActType", string.Empty ) ) );
	}

	private ModifierType ParseModifierType( string inputString ) {

		switch ( inputString ) {

			case "ThornsDmg":
				return ModifierType.ThornsDamage;

			case "SunHPrestore":
				return ModifierType.SunHealthRestore;

			case "BaseMoveSpeed":
				return ModifierType.BaseMoveSpeed;

			case "ManureHPAdd":
				return ModifierType.ManureHealthRestore;

			case "WaterHPrestore":
				return ModifierType.WaterHealthRestore;

			case "DamageKoef":
				return ModifierType.BaseDamage;

			case "BurningTimer":
				return ModifierType.BurningTimerDuration;

			case "Base attack speed":
				return ModifierType.BaseAttackSpeed;

			case "DebuffTimer":
				return ModifierType.DebuffTimerDuration;

			case "BaseRegeneration":
				return ModifierType.BaseAttackSpeed;

			default:
				return ModifierType.None;

		}
	}

	private OffsetValue.OffsetValueType ParseOffsetValueType( string inputString ) {

		switch ( inputString ) {

			case "add":
				return OffsetValue.OffsetValueType.Constant;

			case "mul":
			default:
				return OffsetValue.OffsetValueType.Rate;
		}
	}

}