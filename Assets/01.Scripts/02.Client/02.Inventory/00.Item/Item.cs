using System;
using UnityEngine;
using static Define;
[Serializable]
public class Item
{
    public void Init((int itemType, int count, int category) value)
    {
        itemType = (ItemType)value.itemType;
        count = value.count;
        category = value.category;
    }

    public void SetEctData(string _name, string _info, int _price, Sprite _icon)
    {
        itemName = _name;
        info = _info;
        price = _price;
        icon = _icon;
    }

    public ItemType itemType;
    public int category;
    public int count;

    public string itemName;
    public string info;
    public int price;
    public Sprite icon;


    public int ServerItemData()
    {
        int returnValue = default;
        returnValue |= ((int)itemType).Shift(DataDefine.IntSize.One);
        returnValue |= count.Shift(DataDefine.IntSize.Two);
        returnValue |= category.Shift(DataDefine.IntSize.Three);

        return returnValue;
    }
}
