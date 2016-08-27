using UnityEngine;
using System.Collections;

public class CarStateController : MonoBehaviour {

	public float Speed;
	public float Health;

	public RoomDevice Wheels;
	public RoomDevice Engine;

	public CarStateDebuff[] EveryFrameDebuffs;

	private void Update() {

		for ( var i = 0; i < EveryFrameDebuffs.Length; i++ ) {

			EveryFrameDebuffs[i].ApplyTo( this );
		}
	}

}