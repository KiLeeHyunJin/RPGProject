using System;
using UnityEngine;
using static Define;
[Serializable]
public class Item : IUseable
{
    public void Init((int itemType, int count, int category) value)
    {
        itemType = (ItemType)value.itemType;
        count = value.count;
        category = value.category;
    }

    public void SetEctData(string _name, string _info, int _price, Sprite _icon, int _count)
    {
        count = _count;
        itemName = _name;
        info = _info;
        price = _price;
        icon = _icon;
    }

    ItemType itemType;
    public ItemType ItemType { get { return itemType; } }
    int category;
    public int Category { get { return category; } }
    int count;
    public int Count { get { return count; } }

    string itemName;
    public string ItemName { get { return itemName; } }
    string info;
    public string Info { get { return info; } }
    int price;
    public int Price { get { return price; } }
    Sprite icon;
    public Sprite Icon { get { return icon; } }


    public int ServerItemData()
    {
        int returnValue = default;
        returnValue |= ((int)itemType).Shift(DataDefine.IntSize.One);
        returnValue |= count.Shift(DataDefine.IntSize.Two);
        returnValue |= category.Shift(DataDefine.IntSize.Three);

        return returnValue;
    }
    public virtual void AddItem(int addCount)
    {
        count += addCount;
    }
    public virtual void RemoveItem(int removeCount)
    {
        count -= removeCount;
    }
    public virtual void Used()
    {
    }
}
