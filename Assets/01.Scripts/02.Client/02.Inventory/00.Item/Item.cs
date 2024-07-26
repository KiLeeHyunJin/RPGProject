using static Define;

public class Item
{
    public Item((int itemType, int count, int img, int scriptable) value)
    {
        itemType = (ItemType)value.itemType;
        count = value.count;
        imgData = value.img;
        scriptableData = value.scriptable;
    }

    public string itemName;

    public ItemType itemType;
    public int count;
    public int imgData;
    public int scriptableData;

    public string info;

    public int ServerItemData()
    {
        int returnValue = default;
        returnValue |= ((int)itemType).Shift(DataDefine.IntSize.One);
        returnValue |= count.Shift(DataDefine.IntSize.Two);
        returnValue |= imgData.Shift(DataDefine.IntSize.Three);
        returnValue |= scriptableData.Shift(DataDefine.IntSize.Four);
        return returnValue;
    }
}
