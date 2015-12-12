using UnityEngine;

namespace CharacterFramework.Core {

	public abstract class CharacterPawn : AObject {

		protected float speed;

		[SerializeField]
		private CharacterComplexAnimationController _animationController;

		public void SetSpeed( float newSpeed ) {

			speed = newSpeed;
		}

		public virtual void MoveDirection( Vector3 direction ) {

			transform.position += speed * direction * Time.deltaTime;
		}

		public virtual void SetDestination( Vector3 destination ) {

			//navMeshAgent.destination = destination;
		}

		public virtual float GetDistanceToDestination() {

			return float.NaN;
		}

		//public virtual Vector3 GetDirectionTo( CharacterPawn otherPawn ) {

		//    return Vector3.forward;
		//}

		public void UpdateAnimation( CharacterStateBase currentState ) {

			_animationController.SetBool( currentState.info.AnimatorStateName, true );
		}

	}

	public abstract class CharacterPawn<T> : CharacterPawn where T : CharacterBase {

		public T Character { get; private set; }

		public void SetCharacter( T character ) {

			Character = character;
		}

	}

}