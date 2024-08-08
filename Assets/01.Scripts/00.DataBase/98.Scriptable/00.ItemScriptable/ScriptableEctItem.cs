using UnityEngine;
using static Define;

public class ScriptableEctItem : ScriptableObject
{
    #region set
    public string WarningItemName { set { itemName = value; } }
    public string WarningItemInfo { set { itemInfo = value; } }
    public ItemType WarningItemType { set { itemType = value; } }
    public Sprite WarningIcon { set { imgData = value; } }
    public int WarningPrice { set { price = value; } }
    public int WaringCategory { set { category = value; } }


    #endregion set

    #region get

    public string ItemName { get { return itemName; } }
    public string ItemInfo { get { return itemInfo; } }
    public ItemType ItemType { get { return itemType; } }
    public Sprite Icon { get { return imgData; } }
    public int Price { get { return price; } }
    public int Category { get { return category; } }

    #endregion get

    [SerializeField] private int category;

    [SerializeField] private string itemName;
    [SerializeField] private string itemInfo;
    [SerializeField] private ItemType itemType;
    [SerializeField] private Sprite imgData;
    [SerializeField] private int price;
}
