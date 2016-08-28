using UnityEngine;
using System.Collections;

public class CarStateController : MonoBehaviour {

	public static CarStateController Instance { get; private set; }

	public float Speed;
	public float Health;

	public GlobalGameInfo GlobalGameInfo;

	private void Awake() {

		Instance = this;

		Speed = GlobalGameInfo.StartSpeed;
	}

	void Update() {

		Speed -= GlobalGameInfo.GlobalSpeedLow * Time.deltaTime;

		Speed = Speed.Clamped( 0, GlobalGameInfo.MaxSpeed );
	}

}