using System;
using UniRx;
using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/States/Attack target" )]
public class AttackStateInfo : CharacterStateInfo {

    [Serializable]
    public class State : CharacterState<AttackStateInfo> {

        private Character target;

        public State( CharacterStateInfo info ) : base( info ) {
        }

        public void SetTarget( Character targetCharacter ) {

            if ( targetCharacter != character ) {

                target = targetCharacter;
            }
        }

        public override bool CanBeSet() {

            var weapon = character.inventory.GetArmSlotItem( ArmSlotType.Primary ) as Weapon;

            return target != null && weapon != null && weapon.CanAttack( target );
        }

        public override IEnumerable GetEvaluationBlock() {

            var weapon = character.inventory.GetArmSlotItem( ArmSlotType.Primary ) as Weapon;

            while ( CanBeSet() ) {

                weapon.Attack( target );

                yield return null;
            }
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}