using UnityEngine;
using System.Collections;

public class BurnProjectile : Projectile
{
	public override void OnContact (Collider other)
	{
		var burn = other.GetComponent<ICanBurn>();
		if (burn != null) {
			burn.Burn();
		}
	}
}

