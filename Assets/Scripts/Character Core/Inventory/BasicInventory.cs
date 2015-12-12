using System.Collections.Generic;
using System.Monads;
using UnityEngine;
using System.Collections;

namespace CharacterFramework.Core {

	public class BasicInventory : IInventory {

		public int Gold {
			get { return _gold; }
			set { _gold = value.Clamped( 0, int.MaxValue ); }
		}

		private readonly List<Item> _items = new List<Item>();
		private readonly Dictionary<BodySlotType, Item> _bodySlotsInfo = new Dictionary<BodySlotType, Item>();
		private readonly Dictionary<ArmSlotType, Item> _armSlotsInfo = new Dictionary<ArmSlotType, Item>();
		private readonly Character _character;

		private int _gold;

		public BasicInventory( Character character ) {

			this._character = character;
		}

		public bool AddItem( Item item ) {

			_items.Add( item );

			item.SetCharacter( _character );

			return true;
		}

		public void RemoveItem( Item item ) {

			_items.Remove( item );
		}

		public IEnumerable<Item> GetItems() {

			return _items;
		}

		public bool SetBodySlotItem( BodySlotType bodySlotType, Item item ) {

			if ( _bodySlotsInfo.ContainsKey( bodySlotType ) ) {

				AddItem( _bodySlotsInfo[bodySlotType] );
			}

			_bodySlotsInfo[bodySlotType] = item;
			RemoveItem( item );

			return true;
		}

		public Item GetBodySlotItem( BodySlotType bodySlotType ) {

			return _bodySlotsInfo.With( bodySlotType );
		}

		public bool SetArmSlotItem( ArmSlotType armSlotType, Item item ) {

			if ( _armSlotsInfo.ContainsKey( armSlotType ) ) {

				AddItem( _armSlotsInfo[armSlotType] );
			}

			item.SetCharacter( _character );
			_armSlotsInfo[armSlotType] = item;
			RemoveItem( item );

			return true;
		}

		public Item GetArmSlotItem( ArmSlotType armSlotType ) {

			return _armSlotsInfo.With( armSlotType );
		}

	}

}