using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/Enemy Status Info" )]
public class EnemyCharacterStatusInfo : CharacterStatusInfo {

	public float AggroRadius;
	public float AttackRange;
	public bool CanFriendlyFire;

}
