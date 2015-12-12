using UnityEngine;
using System.Collections;

public class ItemSpawner : AObject {

	public ItemInfo info;

	void Start() {

		Instantiate( info.groundView, position: this.position ).item = info.GetItem();
	}
}
