using System.Collections.Generic;
using Packages.EventSystem;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class DoorAnimationTrigger : MonoBehaviour
    {
		public class StateChange : IEventBase {

			public DoorAnimationTrigger Trigger;

		}

        private List<Animator> _animators = new List<Animator>();
        [SerializeField]
        private BoxCollider _boxCollider;

        [SerializeField]
        private bool _isLocked;

        private bool _fireTrigger;
        private bool _forceOpen;

        public bool IsOpen { get; protected set; }

        private void OnTriggerEnter(Collider collision)
        {
            SetDoorState(collision, true);
        }

        private void OnTriggerStay(Collider collision)
        {
            SetDoorState(collision, true);
        }

        private void OnTriggerExit(Collider collision)
        {
            SetDoorState(collision, false);
        }

        //private methods

        private void SetDoorState(Collider collider, bool state)
        {
            if (_isLocked && AlienLossController.AlienCount != 0) return;
            if (collider == null) return;
            if (_fireTrigger && collider.GetComponent<Projectile>() != null)
            {
                _forceOpen = true;
                SetDoorState(state);
                return;
            }
            if (!_fireTrigger && collider.GetComponent<CharacterPawn>() != null)
            {
                SetDoorState(state);
            }
        }

        public void SetDoorState(bool state)
        {
            if (_forceOpen)
                state = true;
            if (IsOpen == state) return;
            _boxCollider.enabled = !state;
            IsOpen = state;
            _animators.ForEach(x => x.SetBool("Open", IsOpen));

			EventSystem.RaiseEvent( new StateChange { Trigger =  this} );
        }

        private void Awake()
        {
            gameObject.GetComponentsInChildren<Animator>(true, _animators);
            EventSystem.Events.SubscribeOfType<Room.EveryoneDied>(OnEveryoneDieInRoom);
        }

        private void OnEveryoneDieInRoom(Room.EveryoneDied everyoneDiedEvent)
        {
            var room = everyoneDiedEvent.Room;
            if (room.GetRoomType() != Room.RoomType.ControlRoom) return;
            SetDoorState(false);
            _fireTrigger = true;
        }
    }
}
