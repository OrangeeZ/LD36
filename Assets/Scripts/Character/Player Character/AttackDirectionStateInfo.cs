using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/States/Attack direction" )]
public class AttackDirectionStateInfo : CharacterStateInfo {

	private class State : CharacterState<AttackDirectionStateInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override bool CanBeSet() {

			var isButtonDown = Input.GetMouseButton	( 0 );
			return isButtonDown && ( Input.mousePosition - Camera.main.WorldToScreenPoint( character.Pawn.position ) ).magnitude > 0.1f;
			//return GameScreen.instance.attackJoystick.GetValue().magnitude > 0.1f && character.Inventory.GetArmSlotItem( ArmSlotType.Primary ) is Weapon;
		}

		public override IEnumerable GetEvaluationBlock() {

			while ( CanBeSet() ) {

				var weapon = character.Inventory.GetArmSlotItem( ArmSlotType.Primary ) as Weapon;

				var direction = ( Input.mousePosition - Camera.main.WorldToScreenPoint( character.Pawn.position ) );
				direction.z = direction.y;
				direction = direction.Set( y: 0 ).normalized;
                var target = TargetSelector.SelectTarget( character, direction );

				if ( target != null ) {

					weapon.Attack( target );
				} else {

					weapon.Attack( direction );
				}

				yield return null;
			}
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}