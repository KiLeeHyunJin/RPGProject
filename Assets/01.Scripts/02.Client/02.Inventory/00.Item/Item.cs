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

    public long SeverItemData()
    {
        return default;
    }
}
