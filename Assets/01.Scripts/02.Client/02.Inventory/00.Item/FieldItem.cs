using UnityEngine;
using UnityEngine.UI;
using static Define;

public class FieldItem : MonoBehaviour
{
    public Image icon;

    public ItemType itemType;   //아이템 종류
    public int count;           //아이템 개수
    public int itemId;              //아이템 스크립터블
}
