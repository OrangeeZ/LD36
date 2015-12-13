using UnityEngine;
using System.Collections;

public class NPCSpawner : AObject {

	public NPCListInfo NpcListInfo;

	private NPCInfo _currentInfo;

	private void Start() {

		_currentInfo = NpcListInfo.Infos.RandomElement();

		var view = Instantiate( _currentInfo.groundView, position: this.position );
		view.npc = _currentInfo;
	}

	private void OnValidate() {

		name = string.Format( "NPC Spawner [{0}]", _currentInfo == null ? "null" : _currentInfo.name );
	}

}