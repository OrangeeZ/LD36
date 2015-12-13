using UnityEngine;
using System.Collections;

public class NPCSpawner : AObject
{
	public NPCInfo info;

	private void Start()
	{
		var view = Instantiate(info.groundView, position: this.position);
		view.npc = info;
	}

	private void OnValidate() {
		name = string.Format( "NPC Spawner [{0}]", info == null ? "null" : info.name );
	}

}

