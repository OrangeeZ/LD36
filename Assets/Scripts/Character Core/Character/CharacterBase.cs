using UniRx;
using System.Collections.Generic;
using Packages.EventSystem;

namespace CharacterFramework.Core {

	public abstract class CharacterBase {

		public struct Died : IEventBase {

			public CharacterBase character;

		}

		public static List<CharacterBase> instances = new List<CharacterBase>();

		public readonly IntReactiveProperty health;
		public readonly int TeamId;
		protected readonly CharacterStatus status;

		protected CharacterBase( CharacterStatus status, int teamId ) {

			this.status = status;
			this.health = new IntReactiveProperty( this.status.maxHealth.Value );

			this.TeamId = teamId;

			health.Subscribe( OnHealthChange );

			instances.Add( this );
		}

		private void OnHealthChange( int health ) {

			if ( health <= 0 ) {

				EventSystem.RaiseEvent( new Died {character = this} );

				instances.Remove( this );
			}
		}
		public void Dispose() {

			health.Dispose();
		}
	}
}