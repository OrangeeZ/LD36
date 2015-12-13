using UnityEngine;
using System.Collections;

public class NPCSpawner : AObject {

	public NPCListInfo NpcListInfo;
	public NPCInfo OverriderNpcInfo;

	private NPCInfo _currentInfo;

	private void Start() {

		_currentInfo = OverriderNpcInfo == null ? NpcListInfo.Infos.RandomElement() : OverriderNpcInfo;

		var view = Instantiate( _currentInfo.groundView, position: this.position );
		view.npc = _currentInfo;
	}

	private void OnValidate() {

		name = string.Format( "NPC Spawner [{0}]", _currentInfo == null ? "null" : _currentInfo.name );
	}

}