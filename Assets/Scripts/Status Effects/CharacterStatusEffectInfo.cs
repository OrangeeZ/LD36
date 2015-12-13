using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Create/Status effect info")]
public class CharacterStatusEffectInfo : ScriptableObject {

	public int HealthDelta;
	public float MoveSpeedDelta;
	public int AmmoDelta;
	public float ViewRadiusDelta;

	public virtual void Add( CharacterStatus status ) {

		status.MaxHealth.Value += HealthDelta;
		status.MoveSpeed.Value += MoveSpeedDelta;
	}

	public virtual void Remove( CharacterStatus status ) {

		status.MaxHealth.Value -= HealthDelta;
		status.MoveSpeed.Value -= MoveSpeedDelta;
	}
}
