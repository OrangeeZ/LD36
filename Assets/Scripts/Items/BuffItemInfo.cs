using UnityEngine;
using System.Collections;
using csv;

[CreateAssetMenu( menuName = "Create/Items/Buff item info" )]
public class BuffItemInfo : ItemInfo, ICsvConfigurable {

	public ModifierType ModifierType;
	public OffsetValue ModifierValue;

	//[SerializeField]
	//private CharacterStatusEffectInfo _statusEffectInfo;

	private class BuffItem : Item {

		private readonly BuffItemInfo _info;

		public BuffItem( BuffItemInfo info ) : base( info ) {

			_info = info;

		}

		public override void Apply() {

			//_info._statusEffectInfo.Add( Character );
			//Character.Status.AddEffect( _info._statusEffectInfo );

			Character.Status.ModifierCalculator.Add( _info.ModifierType, _info.ModifierValue );

			Character.Inventory.RemoveItem( this );
		}

	}

	public override Item GetItem() {

		return new BuffItem( this );
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