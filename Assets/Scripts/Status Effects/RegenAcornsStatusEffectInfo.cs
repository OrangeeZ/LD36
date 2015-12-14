using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/Status effects/Regen acorns" )]
public class RegenAcornsStatusEffectInfo : CharacterStatusEffectInfo {

	public int MaxAcorns = 10;
	//public float RegenDuration = 0.5f;

	public AcornAmmoItemInfo AcornAmmoItemInfo;

	public override void Add( Character target ) {

		base.Add( target );

		Debug.Log( target );

		new PMonad().Add( RegenAcorns( target ) ).Execute();
	}

	private IEnumerable RegenAcorns( Character target ) {

		var timer = default ( AutoTimer );

		while ( true ) {

			var acornRegenValue = target.Status.ModifierCalculator.CalculateFinalValue( ModifierType.BaseAcornRegen, 0f );
			if ( acornRegenValue <= 0 ) {

				yield return null;

				continue;
			}

			if ( timer != null ) {

				if ( timer.ValueNormalized == 1f ) {

					target.Inventory.AddItem( AcornAmmoItemInfo.GetItem() );

					timer = null;
				}

			} else {

				var regenDuration = 1f / acornRegenValue;
				
				timer = new AutoTimer( regenDuration );
			}

			yield return null;
		}
	}

}