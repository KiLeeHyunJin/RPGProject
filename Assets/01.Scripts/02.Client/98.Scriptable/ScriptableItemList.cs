using System.Collections.Generic;
using UnityEngine;
using static Define;

[CreateAssetMenu]
public class ScriptableItemList : ScriptableObject
{
    [SerializeField] ItemType itemtype;

    [SerializeField] ScriptableEctItem[] itemList;

    public ScriptableEctItem this[int idx] {get { return itemList[idx]; }}
    public ItemType ItemType { get { return itemtype; } }

    public ScriptableEctItem[] WarningItemList { set { itemList = value; } }
    public ItemType WarningItemType { set { itemtype = value; } }

}


