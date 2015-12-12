using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {

    [SerializeField]
    private Text _ammo;

    [SerializeField]
    private Text _lives;

    [SerializeField]
    private Image _reloadingSpinner;

    [SerializeField]
    private AudioClip _reloadSound;

    private Character _character;

    public void SetCharacter( Character character ) {

        _character = character;
    }

    private void Update() {

        if ( _character == null ) {

            return;
        }

        var primaryWeapon = _character.inventory.GetArmSlotItem( ArmSlotType.Primary ) as RangedWeaponInfo.RangedWeapon;

        if ( primaryWeapon == null ) {

            return;
        }

        if ( primaryWeapon.isReloading && !_reloadingSpinner.enabled ) {

            AudioSource.PlayClipAtPoint( _reloadSound, transform.position, 0.3f );
        }

        _reloadingSpinner.enabled = primaryWeapon.isReloading;

        if ( primaryWeapon.isReloading ) {

            _reloadingSpinner.transform.rotation *= Quaternion.AngleAxis( 90f * Time.deltaTime, Vector3.forward );
        }

        _lives.text = _character.health.Value.ToString();
        _ammo.text = primaryWeapon.isReloading ? "--" : primaryWeapon._ammoInClip.ToString();
    }

}