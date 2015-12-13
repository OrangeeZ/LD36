using System;
using UnityEngine;

public abstract class Item {

    public readonly ItemInfo info;

    protected IInventory inventory;

    protected Character character;

    protected Item( ItemInfo info ) {

        this.info = info;
    }

    public void SetCharacter( Character character ) {

        this.character = character;
    }

    public virtual void Apply() {

        throw new NotImplementedException();
    }

}

public class ItemInfo : ScriptableObject {

    public string Name;

    public ItemView groundView;

    public Sprite inventoryIcon;

    public Color color;

    public virtual Item GetItem() {

        throw new NotImplementedException();
    }

    public void DropItem( Transform transform ) {

        var item = GetItem();

        var view = Instantiate( groundView, transform.position, transform.rotation ) as ItemView;

        view.item = item;
        view.SetColor( color );
    }

}