using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/Status effects/Regen acorns" )]
public class RegenAcornsStatusEffectInfo : CharacterStatusEffectInfo {

	public int MaxAcorns = 10;
	public float RegenDuration = 0.5f;

	public AcornAmmoItemInfo AcornAmmoItemInfo;

	public override void Add( Character target ) {

		base.Add( target );

		Debug.Log( target );

		new PMonad().Add( RegenAcorns( target ) ).Execute();
	}

	private IEnumerable RegenAcorns( Character target ) {

		var timer = new AutoTimer( RegenDuration );

		while ( true ) {

			if ( timer.ValueNormalized == 1f ) {

				timer.Reset();

				target.Inventory.AddItem( AcornAmmoItemInfo.GetItem() );
			}

			yield return null;
		}
	}

}