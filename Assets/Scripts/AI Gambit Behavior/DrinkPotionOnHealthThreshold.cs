using System;
using System.Linq;
using UniRx;
using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

namespace AI.Gambits {

	//[Category( "Gambits" )]
	public class DrinkPotionOnHealthThreshold : GambitInfo {

		[Range( 0, 1f )]
		[SerializeField]
		private float threshold = .5f;

		private class DrinkPotionGambit : Gambit {

			private readonly DrinkPotionOnHealthThreshold info;

			private bool canActivate = false;

			public DrinkPotionGambit( Character character, DrinkPotionOnHealthThreshold info )
				: base( character ) {

				this.info = info;
				character.health.Subscribe( UpdateCanActivate );
			}

			public override bool Execute() {

				if ( !canActivate ) {

					return false;
				}

				var item = character.inventory.GetItems().OfType<HealingItemInfo.HealingItem>().FirstOrDefault();

				if ( item == null ) {

					return false;
				}

				item.Apply();

				return true;
			}

			private void UpdateCanActivate( int health ) {

				canActivate = (float)health / character.status.maxHealth.Value <= info.threshold;
			}
		}

		public override Gambit GetGambit( Character target ) {

			return new DrinkPotionGambit( target, this );
		}
	}
}