using System;
using AI.Gambits;
using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/Character Info" )]
public class CharacterInfo : ScriptableObject {

    public CharacterStatusInfo statusInfo;

    public CharacterStateControllerInfo stateControllerInfo;
    public CharacterStateControllerInfo weaponStateControllerInfo;
    public CharacterPawn pawnPrefab;

    public int teamId = 0;

    public bool applyColor = true;

    public virtual Character GetCharacter( Vector3 startingPosition ) {

        var inputSource = new ClickInputSource();
        var pawn = Instantiate( pawnPrefab, startingPosition, Quaternion.identity ) as CharacterPawn;

        var result = new Character(
            pawn,
            inputSource,
            statusInfo.GetInstance(),
            stateControllerInfo.GetStateController(),
            weaponStateControllerInfo.GetStateController(),
            teamId,
            this );

        return result;
    }

}