using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UniRx.Scripts.Ui
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField]
        private Animator _viewAnimator;
        private bool _previousWeaponStatus;
        Character _character;
        private const string _disableState = "Disable";
        private const string _enableState = "Ready";

        public void Initialize(Character character)
        {
            _character = character;
        }

        private void UpdateView()
        {
            var weapon = _character.Inventory.GetArmSlotItem(ArmSlotType.Primary) as RangedWeaponInfo.RangedWeapon;
            if (weapon ==null || _previousWeaponStatus == weapon.IsReloading) return;
            _previousWeaponStatus = weapon.IsReloading;
            if (weapon.IsReloading)
            {
                _viewAnimator.SetTrigger(_disableState);
            }
            else
            {
                _viewAnimator.SetTrigger(_enableState);
            }
        }

        private void Update()
        {
            if (_character == null) return;
            UpdateView();
        }
    }
}
