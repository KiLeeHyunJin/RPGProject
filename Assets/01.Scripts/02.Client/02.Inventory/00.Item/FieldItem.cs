using UnityEngine;
using static Define;

public class FieldItem : PooledObject
{
    Sprite icon;
    public ItemType ItemType { get { return itemType; } }
    ItemType itemType;   //아이템 종류

    public int ItemId { get { return itemId; } }
    int itemId;          //아이템 스크립터블

    public int Count { get { return count; } }
    int count;           //아이템 개수


    SpriteRenderer iconImg;

    public void Init(Sprite _icon, ItemType _itemType, int _itemId, int _count)
    {
        icon = _icon;
        itemType = _itemType;
        itemId = _itemId;
        count = _count;

        iconImg = GetComponentInChildren<SpriteRenderer>();
        if (iconImg != null)
            iconImg.sprite = icon;
    }

}
