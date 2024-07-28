using System;
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

    public ItemType itemType;
    public int category;
    public int count;

    public string itemName;
    public string info;
    public int price;

    public int ServerItemData()
    {
        int returnValue = default;
        returnValue |= ((int)itemType).Shift(DataDefine.IntSize.One);
        returnValue |= category.Shift(DataDefine.IntSize.Two);
        returnValue |= count.Shift(DataDefine.IntSize.Three);
        return returnValue;
    }
}
