using System;
using AI.Gambits;
using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character" )]
public class CharacterInfo : ScriptableObject {

    public CharacterStatusInfo statusInfo;
    public StatExpressionsInfo statExpressionsInfo;

    public CharacterStateControllerInfo stateControllerInfo;
    public CharacterStateControllerInfo weaponStateControllerInfo;
    public CharacterPawn pawnPrefab;

    public GambitListInfo gambitListInfo;

    public int teamId = 0;

    public bool hasInput;

    public bool applyColor = true;

    public virtual Character GetCharacter( Vector3 startingPosition ) {

        var inputSource = hasInput ? (IInputSource) new ClickInputSource() : gambitListInfo.GetGambitList();
        var pawn = Instantiate( pawnPrefab, startingPosition, Quaternion.identity ) as CharacterPawn;

        var result = new Character( statExpressionsInfo,
            pawn,
            inputSource,
            statusInfo.GetInstance( statExpressionsInfo ),
            stateControllerInfo.GetStateController(),
            weaponStateControllerInfo.GetStateController(),
            teamId,
            this );

        if ( !hasInput ) {

            ( inputSource as GambitListInfo.GambitList ).Initialize( result );
        }

        return result;
    }

}