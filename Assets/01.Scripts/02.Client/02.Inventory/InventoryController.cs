using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using static Define;
using static ServerData;

[Serializable]
public partial class InventoryController
{
    public List<List<Item>> slotData;
    public int[] slotCounts;
    public InventoryController(InventoryServerData inventoryData)
    {
        (int equip, int consume, int ect) = inventoryData.ParseSlot();

        slotCounts = new int[] { equip, consume, ect };
        slotData = new((int)ItemType.Non);
 
        for (int i = 0; i < slotData.Capacity; i++)
            slotData[i] = new(slotCounts[i]);

        List<ItemEctServerData> listEct = inventoryData.ect;
        List<ItemEctServerData> listConsume = inventoryData.consume;
        List<ItemEquipServerData> listEquip = inventoryData.equip;

        Item item;

        for (int i = 0; i < slotData[(int)ItemType.Ect].Count; i++)
        {
            item = listEct[i].ExtractItem();
            if(item != null)
            {
                slotData[(int)ItemType.Ect].Add(Manager.Data.GameItemData.GetEctItem(item.itemType, item.category, item.count));
            }
        }
        for (int i = 0; i < slotData[(int)ItemType.Consume].Count; i++)
        {
            item = listConsume[i].ExtractItem();
            if(item != null)
            {
                slotData[(int)ItemType.Consume].Add(Manager.Data.GameItemData.GetConsumeItem(item.itemType, item.category, item.count));
            }
        }

        for (int i = 0; i < slotData[(int)ItemType.Equip].Count; i++)
        {
            Equip equipItem = listEquip[i].ExtractItem();
            if(equipItem != null)
            {
                (Stat upgradeStat, AdditionalStat additional) = listEquip[i].ParseUpgradeStat();
                int possibleCount = equipItem.possableCount;
                equipItem = Manager.Data.GameItemData.GetEquipItem(equipItem.itemType, equipItem.category, equipItem.count);

                equipItem.upgradeStat = upgradeStat;
                equipItem.upgradeAdditional = additional;
                equipItem.possableCount = possibleCount;
            }
            slotData[(int)ItemType.Equip].Add(equipItem);
        }
    }

    public void UseItem(ItemType useType, int idx)
    {

    }
    public int GetSlotCount(ItemType getType)
    {
        if (getType == ItemType.Non)
            return -1;
        return slotCounts[(int)getType];
    }

    public FieldItem DropItem(ItemType type, int idx, int itemId, int count)
    {
        FieldItem drop= new();

        drop.itemType = type;
        drop.itemId = itemId;
        drop.count = count;

        return drop;
    }

    public void GetItem(FieldItem item)
    {
        List<Item> list = slotData[(int)item.itemType];

        int itemCount = item.count;
        ItemType itemType = item.itemType;



        Item input = new();
    }

    public bool AddItem(ItemType type,int idx)
    {
        if (slotData[(int)type].Count <= slotCounts[idx])
            return false;

        return true;
    }

    public void RemoveItem(ItemType type, int idx)
    {
        if (slotData[(int)type][idx] != null)
            slotData[(int)type][idx] = null;
    }

    public void SwapItem()
    {

    }

}
