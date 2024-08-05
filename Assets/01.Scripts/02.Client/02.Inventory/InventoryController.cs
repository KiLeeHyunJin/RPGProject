using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;
using static ServerData;

[Serializable]
public partial class InventoryController
{
    UserCharacterController characterController;
    public List<List<Item>> slotData;
    public Equip[] wearSlotData;
    public int[] slotCounts;
    public InventoryController(UserCharacterController owner, InventoryServerData inventoryData)
    {
        characterController = owner;
        (int equip, int consume, int ect) = inventoryData.ParseSlot();

        slotCounts = new int[] { equip, consume, ect };
        slotData = new((int)ItemType.Non);
 
        for (int i = 0; i < slotData.Capacity; i++)
            slotData[i] = new(slotCounts[i]);

        wearSlotData = new Equip[(int)EquipType.END];

        List<ItemEctServerData> listEct = inventoryData.ect;
        List<ItemEctServerData> listConsume = inventoryData.consume;
        List<ItemEquipServerData> listEquip = inventoryData.equip;

        int category;
        int count;
        int itemType;

        for (int i = 0; i < slotData[(int)ItemType.Ect].Count; i++)
        {
            (itemType, count, category) = listEct[i].ParseCode();
            if (count > 0)
            {
                slotData[(int)ItemType.Ect].Add(Manager.Data.GameItemData.GetEctItem((Define.ItemType)itemType, category, count));
            }
        }
        for (int i = 0; i < slotData[(int)ItemType.Consume].Count; i++)
        {
            (itemType, count, category) = listConsume[i].ParseCode();
            if(count > 0) 
            {
                slotData[(int)ItemType.Consume].Add(Manager.Data.GameItemData.GetConsumeItem((Define.ItemType)itemType, category, count));
            }
        }

        for (int i = 0; i < slotData[(int)ItemType.Equip].Count; i++)
        {
            Equip equipItem = listEquip[i].ExtractItem();
            slotData[(int)ItemType.Equip].Add(equipItem);
        }
    }

    public void UseItem(ItemType useType, int idx)
    {
        if (useType == ItemType.Equip || 
            useType == ItemType.Consume)
        {
            slotData[(int)useType][idx]?.Used();
        }
    }

    public void SetKeyUsedType(Key keyCode)
    {
        characterController.KeyController.ChangeInteractionRemove(keyCode);
    }

    public KeyController.KeyActionCallbackBundle GetUsedItemKeyAttachCallback(int idx)
    {
        int callIdx = idx;
        return new KeyController.KeyActionCallbackBundle(_performed : (call) => UseItem(ItemType.Consume, callIdx));
    }

    public int GetSlotCount(ItemType getType)
    {
        if (getType == ItemType.Non)
            return -1;
        return slotCounts[(int)getType];
    }

    public FieldItem DropItem(ItemType type, int idx, int itemId, int count)
    {
        FieldItem drop = new();

        drop.Init(slotData[(int)type][idx].Icon,type, itemId, count);

        return drop;
    }

    public bool GetItem(FieldItem item)
    {
        ItemType type = item.ItemType;
        int category = item.ItemId;
        int count = item.Count;

        if(int.Equals(type, (int)ItemType.Equip) == false)
        {
            int findIndex = FindItem(item.ItemType, category);
            if (findIndex >= 0)
            {
                slotData[(int)type][findIndex].AddItem(count);
                return true;
            }
        }


        //비어있는 슬롯이 있는지 탐색

        Item getItem = null;
        switch (type)
        {
            case ItemType.Equip:
                getItem = Manager.Data.GameItemData.GetEquipItem(type, count, category);
                break;
            case ItemType.Consume:
                getItem = Manager.Data.GameItemData.GetConsumeItem(type, count, category);
                break;
            case ItemType.Ect:
                getItem = Manager.Data.GameItemData.GetEctItem(type, count, category);
                break;
            default:
                break;
        }
        if (getItem == null)
        {
            return false;
        }
        //비어있는 슬롯에 아이템 추가

        return true;
    }



    //슬롯을 찾으면 해당 슬롯 번호 반환
    //없을 경우 -1
    int FindItem(ItemType type, int id)
    {
        List<Item> searchList = slotData[(int)type];
        for (int i = 0; i < searchList.Count; i++)
        {
            if (int.Equals(searchList[i].Category,id))
            {
                return i;
            }
        }
        return -1;
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
