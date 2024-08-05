using System;
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

    private void SetBaseData(
        Action<(int itemType, int count, int category)> Init, 
        Action<string, string, int, Sprite, int> SetEctData, 
        Item item, ScriptableEctItem scriptable, int count)
    {

        Init(((int)item.ItemType, count, item.Category));
        SetEctData(scriptable.ItemName, scriptable.ItemInfo, scriptable.Price, scriptable.Icon, count);
    }

    public Item GetItem(ItemType type, int count, int category)
    {
        Item item = null;

        Action<(int itemType, int count, int category)> _Init;
        Action<string, string, int, Sprite, int> _SetEctData;



        switch (type)
        {
            case ItemType.Equip:
                Action<EquipType, int, Stat, Stat, AdditionalStat> SetEquip;
                Action<int, Stat, AdditionalStat> _InitServerData;
                Equip equip = new(out _Init,out _SetEctData, out SetEquip, out _InitServerData);
                ScriptableEquipItem equipScriptable = (ScriptableEquipItem)dataBase[(int)type][category];
                if (equipScriptable == null)
                {
                    Message.LogWarning($"Get Equip Item Is Failed - Category Num : {category}");
                    return null;
                }
                SetBaseData(_Init, _SetEctData, equip, equipScriptable, count);
                SetEquip(equipScriptable.WearType, equipScriptable.Level, equipScriptable.LimitStat, equipScriptable.BaseStat, equipScriptable.BaseAdditional);
                item = equip;
                break;

            case ItemType.Consume:
                Action<ConsumeType, ConsumeEffectType, int, int> SetConsumeData;

                Consume consume = new(out _Init, out _SetEctData, out SetConsumeData);
                ScriptableConsumeItem consumeScriptable = (ScriptableConsumeItem)dataBase[(int)type][category];
                if (consumeScriptable == null)
                {
                    Message.LogWarning($"Get Consume Item Is Failed - Category Num : {category}");
                    return null;
                }
                SetBaseData(_Init, _SetEctData, consume, consumeScriptable, count);
                SetConsumeData(consumeScriptable.UseType, consumeScriptable.EfxType, consumeScriptable.Value, consumeScriptable.DuringValue);
                item = consume;
                break;

            case ItemType.Ect:
                Item ect = new(out _Init, out _SetEctData);
                ScriptableEctItem ectcriptable = dataBase[(int)type][category];
                if (ectcriptable == null)
                {
                    Message.LogWarning($"Get Ect Item Is Failed - Category Num : {category}");
                    return null;
                }
                SetBaseData(_Init, _SetEctData, ect, ectcriptable, count);
                item = ect;
                break;
            case ItemType.Non:
                break;
            default:
                break;
        }
        return item;
    }

    //public Item GetEctItem(ItemType type, int count, int category)
    //{
    //    Item item = new();
    //    ScriptableEctItem scriptable = dataBase[(int)type][category];
    //    if (scriptable == null)
    //    {
    //        Message.LogWarning($"Get Ect Item Is Failed - Category Num : {category}");
    //        return null;
    //    }
    //    SetBaseData(item, scriptable, count);

    //    return item;
    //}

    //public Consume GetConsumeItem(ItemType type,int count, int category)
    //{
    //    Consume item = new();
    //    ScriptableConsumeItem scriptable = (ScriptableConsumeItem)dataBase[(int)type][category];
    //    if(scriptable == null)
    //    {
    //        Message.LogWarning($"Get Consume Item Is Failed - Category Num : {category}");
    //        return null;
    //    }
    //    SetBaseData(item, scriptable, count);

    //    item.SetConsumeData(scriptable.UseType, scriptable.EfxType, scriptable.Value, scriptable.DuringValue);
    //    return item;
    //}

    public (Equip, Action<int, Stat, AdditionalStat>) GetEquipItem(ItemType type, int count, int category)
    {
        Action<(int itemType, int count, int category)> _Init;
        Action<string, string, int, Sprite, int> _SetEctData;
        Action<EquipType, int, Stat, Stat, AdditionalStat> SetEquip;
        Action<int, Stat, AdditionalStat> InitServerData;
        Equip item = new(out _Init, out _SetEctData, out SetEquip, out InitServerData);

        ScriptableEquipItem scriptable = (ScriptableEquipItem)dataBase[(int)type][category];
        if (scriptable == null)
        {
            Message.LogWarning($"Get Equip Item Is Failed - Category Num : {category}");
            return (null, null);
        }
        SetBaseData(_Init, _SetEctData, item, scriptable, count);

        SetEquip(scriptable.WearType, scriptable.Level, scriptable.LimitStat, scriptable.BaseStat, scriptable.BaseAdditional);
        return (item, InitServerData);
    }



}

