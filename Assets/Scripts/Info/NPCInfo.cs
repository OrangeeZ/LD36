﻿using UnityEngine;
using System.Collections;

public class NPCInfo : ScriptableObject, ICsvConfigurable
{
	public ItemInfo itemInfo;
	public NPCView groundView;

	public void Configure (csv.Values values)
	{
		var weapon = values.GetScriptableObject<ItemInfo>("ChangeWeapon");
		var ability = values.GetScriptableObject<ItemInfo>("Ability");
		itemInfo = weapon ?? ability;
	}
}
