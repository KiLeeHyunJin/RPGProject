using UnityEngine;
using UnityEngine.UI;
using static Define;

public class FieldItem : MonoBehaviour
{
    Sprite icon;
    ItemType itemType;   //아이템 종류
    public ItemType ItemType { get { return itemType; } }
    int itemId;          //아이템 스크립터블
    public int ItemId { get { return itemId; } }
    int count;           //아이템 개수
    public int Count { get { return count; } }


    public void Init(Sprite _icon, ItemType _itemType, int _itemId, int _count)
    {
        icon = _icon;
        itemType = _itemType;
        itemId = _itemId;
        count = _count;
    }

}
