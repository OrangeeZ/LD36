using UnityEngine;
using System.Collections;
using csv;

public class ZoneInfo : ScriptableObject, ICsvConfigurable {

	public float HealthPool;
	public float HealingSpeed;

	public ZoneView ZonePrefab;

	public void Configure( Values values ) {

		HealingSpeed = values.Get( "HpPerSec", 0f );
		HealthPool = values.Get( "HpPool", 0f );
		ZonePrefab = values.GetPrefabWithComponent<ZoneView>( "Visual", fixName: false );
	}

}
