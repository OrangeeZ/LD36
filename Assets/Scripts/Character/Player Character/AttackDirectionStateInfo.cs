using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character states" )]
public class AttackDirectionStateInfo : CharacterStateInfo {

    private class State : CharacterState<AttackDirectionStateInfo> {

        public State( CharacterStateInfo info ) : base( info ) {
        }

        public override bool CanBeSet() {

            return GameScreen.instance.attackJoystick.GetValue().magnitude > 0.1f && character.inventory.GetArmSlotItem( ArmSlotType.Primary ) is Weapon;
        }

        public override IEnumerable GetEvaluationBlock() {

            while ( CanBeSet() ) {

                var weapon = character.inventory.GetArmSlotItem( ArmSlotType.Primary ) as Weapon;

                var target = TargetSelector.SelectTarget( character, GameScreen.instance.attackJoystick.GetValue() );

                if ( target != null ) {

                    weapon.Attack( target );
                } else {

                    weapon.Attack( GameScreen.instance.attackJoystick.GetValue() );
                }

                yield return null;
            }
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}