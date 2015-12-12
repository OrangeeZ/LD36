using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UniRx;

public class DangerPanel : MonoBehaviour {

    [SerializeField]
    private Slider _slider;

    void Start() {

        GameplayController.instance.dangerLevel.Subscribe( OnDangerValueChange );
        _slider.maxValue = GameplayController.maxDanger;
    }

    private void OnDangerValueChange( int value ) {

        _slider.value = value;
    }
}
