using System;
using UnityEngine;
using static Define;
[Serializable]
public class Item : IUseable
{
    public Item(out Action<(int itemType, int count, int category)> _Init,out Action<string, string, int, Sprite, int> _SetEctData)
    {
        _Init = Init;
        _SetEctData = SetEctData;
    }
    void Init((int itemType, int count, int category) value)
    {
        itemType = (ItemType)value.itemType;
        count = value.count;
        category = value.category;
    }

    void SetEctData(string _name, string _info, int _price, Sprite _icon, int _count)
    {
        count = _count;
        itemName = _name;
        info = _info;
        price = _price;
        icon = _icon;
    }

    public ItemType ItemType { get { return itemType; } }
    ItemType itemType;
    public int Category { get { return category; } }
    int category;
    public int Count { get { return count; } }
    int count;
    public string ItemName { get { return itemName; } }
    string itemName;
    public string Info { get { return info; } }
    string info;
    public int Price { get { return price; } }
    int price;
    public Sprite Icon { get { return icon; } }
    Sprite icon;


    public int ServerItemData()
    {
        int returnValue = default;
        returnValue |= ((int)itemType).Shift(DataDefine.IntSize.One);
        returnValue |= count.Shift(DataDefine.IntSize.Two);
        returnValue |= category.Shift(DataDefine.IntSize.Three);

        return returnValue;
    }
    public void AddItem(int addCount)
    {
        count += addCount;
    }

    public void MinuseItem(int removeCount)
    {
        if(Count < removeCount)
        {
            Message.LogError($"{itemName}의 소지 개수를 넘어서는 차감이 실행되었습니다. ");
        }
        else if(int.Equals(removeCount, Count))
        {
            RemoveItem();
            return;
        }
        count -= removeCount;
    }
    public virtual void Used()
    {

    }

    protected virtual void RemoveItem()
    {
        itemType = ItemType.Non;

        category = default;
        count = default;
        price = default;

        itemName = null;
        info = null;
        icon = null;
    }


}
