using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Car State Debuffs/Obstacle Slowdown")]
public class ObstacleSlowdownDebuff : CarStateDebuff {

	public override void ApplyTo( CarStateController carStateController ) {

		base.ApplyTo( carStateController );

		var wheelsDeviceState = carStateController.Wheels.CurrentState;

		if ( wheelsDeviceState == RoomDevice.State.Broken ) {

			carStateController.Speed -= BrokenPenalty;
		} else {

			carStateController.Speed -= IntactPenalty;
		}
	}

}

