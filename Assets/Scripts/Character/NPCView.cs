using UnityEngine;
using System.Collections;

public class NPCView : AObject
{
	public Transform itemSpawnPoint;
	public NPCInfo npc;
	public bool destroyOnPickup;

	public ItemView itemView { get; private set; }

	void Start()
	{
		itemView = npc.itemInfo.DropItem (itemSpawnPoint ?? transform);
		itemView.giver = this;
	}

	public void OnPickedUp()
	{
		if (destroyOnPickup) {
			Destroy(gameObject);
		}
	}
}

