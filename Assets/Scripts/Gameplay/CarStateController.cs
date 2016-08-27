using UnityEngine;
using System.Collections;

public class CarStateController : MonoBehaviour {

	public static CarStateController Instance { get; private set; }

	public float Speed;
	public float Health;

	public RoomDevice Wheels;
	public RoomDevice Engine;

	public CarStateDebuff[] EveryFrameDebuffs;

	private void Awake() {

		Instance = this;
	}

	private void Update() {

		for ( var i = 0; i < EveryFrameDebuffs.Length; i++ ) {

			EveryFrameDebuffs[i].ApplyTo( this );
		}
	}

}