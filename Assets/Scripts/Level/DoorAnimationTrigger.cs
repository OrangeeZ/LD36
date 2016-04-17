using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class DoorAnimationTrigger : MonoBehaviour
    {
        private List<Animator> _animators = new List<Animator>();

        public bool IsOpen { get; protected set; }

        private void OnCollisionEnter(Collision collision)
        {
            SetDoorState(collision.collider, true);
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            SetDoorState(collisionInfo.collider, true);
        }

        private void OnCollisionExit(Collision collision)
        {
            SetDoorState(collision.collider, false);
        }

        //private methods

        private void SetDoorState(Collider collider, bool state)
        {
            if (IsOpen == state || collider == null) return;
            var component = collider.GetComponent<CharacterPawn>();
            if (component == null) return;
            IsOpen = true;
            _animators.ForEach(x => x.SetBool("Open", IsOpen));
        }

        private void Awake()
        {
            gameObject.GetComponentsInChildren<Animator>(true, _animators);
        }

    }
}
