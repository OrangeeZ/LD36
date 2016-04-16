using System;
using UI.uGui.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UniRx.Scripts.Ui
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField]
        private Animator _viewAnimator;
        [SerializeField]
        private Text _charches;
        [SerializeField]
        private ProgressAnimation _progressAnimation;

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

            if (weapon ==null)return;
            if (weapon.IsUnlimited)
                _charches.text = "∞";
            else
            {
                _charches.text = weapon.AmmoInClip.ToString();
            }
            if (_previousWeaponStatus == weapon.IsReloading) return;
            _previousWeaponStatus = weapon.IsReloading;
            if (weapon.IsReloading)
            {
                _progressAnimation.StartAnimation(0,1,weapon.ReloadDuration);
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
