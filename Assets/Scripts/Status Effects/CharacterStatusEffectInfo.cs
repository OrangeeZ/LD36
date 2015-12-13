using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Create/Status effect info")]
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
	}
}
