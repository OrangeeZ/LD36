using UnityEngine;
using System.Collections;

public class CarStateController : MonoBehaviour {

	public static CarStateController Instance { get; private set; }

	public float Speed;
	public float Health;

	public RoomDevice Wheels;
	public RoomDevice Engine;

	private void Awake() {

		Instance = this;
	}

}