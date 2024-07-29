using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static Define;

public class CustomItemBundle
{
    public static string[] scriptablePath = { $"{Define.ItemScripatablePath}"};

    [UnityEditor.MenuItem("Tools/ItemList")]
    public static void RefresgItemList()
    {
        UnityEngine.Debug.ClearDeveloperConsole();

        bool[] checkItemList = new bool[(int)ItemType.Non];
        for (int i = 0; i < checkItemList.Length; i++)
            checkItemList[i] = false;

        var allObjectGuids = AssetDatabase.FindAssets("t:ScriptableItemList", scriptablePath);
        if (allObjectGuids == null)
            return;
        foreach (var guid in allObjectGuids)
        {
            ScriptableItemList scriptableItemList = AssetDatabase.LoadAssetAtPath<ScriptableItemList>(AssetDatabase.GUIDToAssetPath(guid));
            ItemType checkItemType = scriptableItemList.ItemType;
            if (checkItemList[(int)checkItemType] == false)
            {
                checkItemList[(int)checkItemType] = true;
                RefreshItemList(scriptableItemList);
            }
            else
            {
                Message.Log($"Remove ItemBundle List {scriptableItemList.name}");
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AssetDatabase.DeleteAsset(path);
                continue;
            }
        }
        for (int i = 0; i < checkItemList.Length; i++)
        {
            if (checkItemList[i] == false)
            {
                ScriptableItemList asset = ScriptableObject.CreateInstance<ScriptableItemList>();
                string path = $"{scriptablePath[0]}/{(ItemType)i}.asset";
                
                AssetDatabase.CreateAsset(asset, path);
                asset.WarningItemType = (ItemType)i;
                RefreshItemList(asset);
            }
        }
    }

    static void RefreshItemList(ScriptableItemList scriptableItemList)
    {
        if(scriptableItemList.ItemType == ItemType.Non)
            return;

        string[] path = { $"{scriptablePath[0]}{ItemBundleSearchPath(scriptableItemList.ItemType)}" };
        var allObjectGuids = AssetDatabase.FindAssets("t:ScriptableEctItem", path);
        if (allObjectGuids == null || allObjectGuids.Length == 0)
            return;

        Message.Log($"Create Array {scriptableItemList.ItemType}");
        ScriptableEctItem[] itemList = new ScriptableEctItem[ItemBundleSize];
       
        foreach (var guid in allObjectGuids)
        {
            ScriptableEctItem scriptableItem = AssetDatabase.LoadAssetAtPath<ScriptableEctItem>(AssetDatabase.GUIDToAssetPath(guid));

            Message.Log($"{scriptableItem.name}");
            if (itemList[scriptableItem.Category] != null)
            {
                Message.LogWarning($"itemList[scriptableItem.Category] != null : crash  \n {itemList[scriptableItem.Category].name} & {scriptableItem.name}");
                continue;
            }
            itemList[scriptableItem.Category] = scriptableItem;
        }
        scriptableItemList.WarningItemList = itemList;
    }

    static string ItemBundleSearchPath(ItemType checkItemType)
    {
        return checkItemType switch
        {
            ItemType.Equip => Define.ItemScripatableEquipPath,
            ItemType.Consume => Define.ItemScripatableConsumePath,
            ItemType.Ect => Define.ItemScripatableEctPath,
            ItemType.Non => null,
            _ => null,
        };
    }

}
