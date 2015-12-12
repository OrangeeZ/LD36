
using System.Collections.Generic;

public enum BodySlotType {

	Body,
	Head,
	Legs
}

public enum ArmSlotType {

	Primary,
	Secondary
}

public interface IInventory {

	int Gold { get; set; }

	bool AddItem( Item item );

	void RemoveItem( Item item );

	IEnumerable<Item> GetItems();

	bool SetBodySlotItem( BodySlotType bodySlotType, Item item );

	Item GetBodySlotItem( BodySlotType bodySlotType );

	bool SetArmSlotItem( ArmSlotType armSlotType, Item item );

	Item GetArmSlotItem( ArmSlotType armSlotType );
}

//[System.Serializable]
//public class IInventoryContainer : IUnifiedContainer<IInventory> { }
