﻿using System.Collections.Generic;
using Packages.EventSystem;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class DoorAnimationTrigger : MonoBehaviour
    {
		public class StateChange : IEventBase {

			public DoorAnimationTrigger Trigger;

		}

        private List<Animator> _animators = new List<Animator>();

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
            if (IsOpen == state || collider == null) return;
            var component = collider.GetComponent<CharacterPawn>();
            if (component == null) return;
            IsOpen = state;
            _animators.ForEach(x => x.SetBool("Open", IsOpen));

			EventSystem.RaiseEvent( new StateChange { Trigger =  this} );
        }

        private void Awake()
        {
            gameObject.GetComponentsInChildren<Animator>(true, _animators);
        }

    }
}
