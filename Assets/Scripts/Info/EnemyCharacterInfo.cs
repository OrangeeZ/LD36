using UnityEngine;
using System.Collections;
using AI.Gambits;

[CreateAssetMenu( menuName = "Create/Enemy Character Info" )]
public class EnemyCharacterInfo : CharacterInfo {

	public EnemyCharacterStatusInfo EnemyStatusInfo;

	public GambitListInfo GambitListInfo;

	public override Character GetCharacter( Vector3 startingPosition ) {

		var inputSource = GambitListInfo.GetGambitList();
		var pawn = Instantiate( pawnPrefab, startingPosition, Quaternion.identity ) as CharacterPawn;

		var result = new Character(
			pawn,
			inputSource,
			EnemyStatusInfo.GetInstance(),
			stateControllerInfo.GetStateController(),
			weaponStateControllerInfo.GetStateController(),
			teamId,
			this );

		inputSource.Initialize( result );

		return result;
	}

}