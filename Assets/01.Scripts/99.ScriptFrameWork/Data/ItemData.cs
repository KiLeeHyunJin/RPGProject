using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemData
{
    private readonly ScriptableItemDataBase dataBase;
    public ItemData(ScriptableItemDataBase _dataBase)
    {
        dataBase = _dataBase;
    }

    private void SetBaseData(Item item, ScriptableEctItem scriptable, int count)
    {
        item.SetEctData(scriptable.ItemName, scriptable.ItemInfo, scriptable.Price, scriptable.Icon, count);
    }

    public Item GetEctItem(ItemType type, int count, int category)
    {
        Item item = new();
        ScriptableEctItem scriptable = dataBase[(int)type][category];
        if (scriptable == null)
        {
            Message.LogWarning($"Get Ect Item Is Failed - Category Num : {category}");
            return null;
        }
        SetBaseData(item, scriptable, count);

        return item;
    }

    public Consume GetConsumeItem(ItemType type,int count, int category)
    {
        Consume item = new();
        ScriptableConsumeItem scriptable = (ScriptableConsumeItem)dataBase[(int)type][category];
        if(scriptable == null)
        {
            Message.LogWarning($"Get Consume Item Is Failed - Category Num : {category}");
            return null;
        }
        SetBaseData(item, scriptable, count);

        item.SetConsumeData(scriptable.UseType, scriptable.EfxType, scriptable.Value, scriptable.DuringValue);
        return item;
    }

    public Equip GetEquipItem(ItemType type, int count, int category)
    {
        Equip item = new();
        ScriptableEquipItem scriptable = (ScriptableEquipItem)dataBase[(int)type][category];
        if (scriptable == null)
        {
            Message.LogWarning($"Get Equip Item Is Failed - Category Num : {category}");
            return null;
        }
        SetBaseData(item, scriptable, count);

        item.SetEquipData(scriptable.WearType, scriptable.Level, scriptable.LimitStat,scriptable.BaseStat, scriptable.BaseAdditional);
        return item;
    }



}
