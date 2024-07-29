using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ScriptableEctItem : ScriptableObject
{
    #region set
    public string WarningItemName { set { itemName = value; } }
    public string WarningItemInfo { set { itemInfo = value; } }
    public ItemType WarningItemType { set { itemType = value; } }
    public int WarningCount { set { count = value; } }
    public Sprite WarningImgData { set { imgData = value; } }
    public int WarningScriptableData { set { scriptableData = value; } }
    public int WarningPrice { set { price = value; } }
    public int WaringCategory { set { category = value; } }

    #endregion set

    #region get

    public string ItemName { get { return itemName; } }
    public string ItemInfo { get { return itemInfo; } }
    public ItemType ItemType { get { return itemType; } }
    public int Count { get { return count; } }
    public Sprite ImgData { get { return imgData; } }
    public int ScriptableData { get { return scriptableData; } }
    public int Price { get { return price; } }
    public int Category { get { return category; } }

    #endregion get

    [SerializeField] private int category;

    [SerializeField] private string itemName;
    [SerializeField] private string itemInfo;
    [SerializeField] private ItemType itemType;
    [SerializeField] private int count;
    [SerializeField] private Sprite imgData;
    [SerializeField] private int scriptableData;
    [SerializeField] private int price;
}
