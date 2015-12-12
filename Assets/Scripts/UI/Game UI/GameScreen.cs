using UnityEngine;
using System.Collections;

public class GameScreen : UIScreen {
    
    public static GameScreen instance { get; private set; }

    public UIJoystick moveJoystick;
    public UIJoystick attackJoystick;

    public StatsPanel statsPanel;

    void Awake() {

        instance = this;
    }
}
