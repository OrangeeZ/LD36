using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Car State Debuffs/Engine Slowdown")]
public class EngineSlowdownDebuff : CarStateDebuff {

	public override void ApplyTo( CarStateController carStateController ) {

		base.ApplyTo( carStateController );

		var wheelsDeviceState = carStateController.Engine.CurrentState;

		if ( wheelsDeviceState == RoomDevice.State.Broken ) {

			carStateController.Speed -= BrokenPenalty * Time.deltaTime;
		} else {

			carStateController.Speed -= IntactPenalty * Time.deltaTime;
		}
	}

}

