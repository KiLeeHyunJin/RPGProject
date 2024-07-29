using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Define;

public class CustomItemBundle
{
    public static string[] scriptablePath = { $"{Define.ItemScripatablePath}" };

    [UnityEditor.MenuItem("Tools/ItemList")]
    public static void RefresgItemList()
    {
        UnityEngine.Debug.ClearDeveloperConsole();
        List<ScriptableItemList> dataList = new((int)ItemType.Non);
        for (int i = 0; i < dataList.Capacity; i++)
        {
            dataList.Add(null);
        }
        bool[] checkItemList = new bool[(int)ItemType.Non];
        checkItemList.DefaultValue();

        var allObjectGuids = AssetDatabase.FindAssets("t:ScriptableItemList", scriptablePath);
        var dataBase = AssetDatabase.FindAssets("t:ScriptableItemDataBase", scriptablePath);

        ScriptableItemDataBase ItemdataBase;
        if (dataBase.Length !=  0)
        {
            string scripablePath = AssetDatabase.GUIDToAssetPath(dataBase[0]);
            ItemdataBase = AssetDatabase.LoadAssetAtPath<ScriptableItemDataBase>(scripablePath);
        }
        else
        {
            ItemdataBase = ScriptableObject.CreateInstance<ScriptableItemDataBase>();
            string path = $"{scriptablePath[0]}/ItemListBundle.asset";
            Debug.Log(path);
            AssetDatabase.CreateAsset(ItemdataBase, path);
        }


        foreach (var guid in allObjectGuids)
        {
            string scripablePath = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableItemList scriptableItemList = AssetDatabase.LoadAssetAtPath<ScriptableItemList>(scripablePath);
            ItemType checkItemType = scriptableItemList.ItemType;
            if (checkItemList[(int)checkItemType] == false)
            {
                checkItemList[(int)checkItemType] = true;
                RefreshItemList(scriptableItemList);

                string expectedName = $"{checkItemType}_List";

                // 아이템 리스트의 이름 설정
                if (!string.Equals(scriptableItemList.name, expectedName))
                {
                    AssetDatabase.RenameAsset(scripablePath, expectedName);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            else
            {
                Message.Log($"Remove ItemBundle List {scriptableItemList.name}");
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AssetDatabase.DeleteAsset(path);
                continue;
            }
            dataList[(int)checkItemType] = (scriptableItemList);
        }

        for (int i = 0; i < checkItemList.Length; i++)
        {
            if (checkItemList[i] == false)
            {
                ScriptableItemList asset = ScriptableObject.CreateInstance<ScriptableItemList>();
                string path = $"{scriptablePath[0]}/{(ItemType)i}_List.asset";

                AssetDatabase.CreateAsset(asset, path);
                asset.WarningItemType = (ItemType)i;
                RefreshItemList(asset);
                dataList[i] = asset;
            }
        }
        ItemdataBase.WarningItemList = dataList;
    }

    static void RefreshItemList(ScriptableItemList scriptableItemList)
    {
        if (scriptableItemList.ItemType == ItemType.Non)
            return;

        string[] path = { $"{scriptablePath[0]}{ItemBundleSearchPath(scriptableItemList.ItemType)}" };
        var allObjectGuids = AssetDatabase.FindAssets("t:ScriptableEctItem", path);
        if (allObjectGuids == null || allObjectGuids.Length == 0)
            return;

        ScriptableEctItem[] itemList = new ScriptableEctItem[ItemBundleSize];

        foreach (var guid in allObjectGuids)
        {
            ScriptableEctItem scriptableItem = AssetDatabase.LoadAssetAtPath<ScriptableEctItem>(AssetDatabase.GUIDToAssetPath(guid));

            //Message.Log($"{scriptableItem.name}");
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
