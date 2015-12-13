using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class Burning : MonoBehaviour
{
	public float spreadLowering = 0.5f;

	[Header("Spread")]
	public float spreadRange = 1;
	public float spreadChance = 0.1f;

	[Space(20)]
	//public object effect; DOT info
	public float stacks = 1f;	// power
	public float stackDuration = 1f;	// power

	public float maxStacks = 10;
	public float visualScaleMax = 4f;
	public GameObject visualPrefab;

	private GameObject visual;
	private ParticleSystemMultiplier visualScaler;

	void Start()
	{
		visual = GameObject.Instantiate(visualPrefab);
		visual.transform.parent = transform;
		visual.transform.localPosition = Vector3.zero;

		visualScaler = visual.GetComponent<ParticleSystemMultiplier>();

		StartCoroutine(Spreading());
	}

	IEnumerator Spreading()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);

			stacks -= 1f / stackDuration;

			if (stacks <= 0) {
				Stop ();
				yield break;
			}

			visualScaler.SetScale(Mathf.Clamp01(stacks / maxStacks) * visualScaleMax);

			float chance = Random.Range(0f, 1f) * stacks;
			if (chance >= 1 - spreadChance) {
				Spread();
			}
		}
	}

	void Stop ()
	{
		enabled = false;
		Destroy(visual);
		Destroy (this);
	}

	void Spread()
	{
		var objects = GameObject.FindObjectsOfType<ICanBeAffected>();

		for (int i = 0; i < objects.Length; i++) {
			var obj = objects[i];
			var dist = transform.position - obj.transform.position;
			if (dist.magnitude <= spreadRange) {
				var burning = obj.GetComponent<Burning>();
				if (burning != null) {
					burning.stacks++;
					float factor = 1f / burning.stacks;
					burning.spreadChance = Mathf.Lerp(burning.spreadChance, spreadChance, factor);
					burning.enabled = true;
				} else {
					burning = obj.gameObject.AddComponent<Burning>();
					burning.visualPrefab = visualPrefab;
					burning.spreadRange = spreadRange;
					burning.spreadChance = spreadChance * spreadLowering;
					burning.stacks = 1f;
				}
			}
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, spreadRange);
	}
}

