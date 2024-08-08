using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptableItemDataBase : ScriptableObject
{
    [SerializeField] List<ScriptableItemList> itemList;
    public ScriptableItemList this[int idx] { get { return itemList[idx]; } }
    public List<ScriptableItemList> WarningItemList { set { itemList = value; } }
}
