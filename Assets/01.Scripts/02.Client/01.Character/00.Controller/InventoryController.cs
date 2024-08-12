using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;
using static ServerData;

[Serializable]
public class InventoryController
{
    readonly UserCharacterController characterController;
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
            slotData.Add(new(slotCounts[i]));

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
                slotData[(int)ItemType.Ect].Add(Manager.Data.GameItemData.GetItem((Define.ItemType)itemType, category, count));
            }
        }
        for (int i = 0; i < slotData[(int)ItemType.Consume].Count; i++)
        {
            (itemType, count, category) = listConsume[i].ParseCode();
            if (count > 0)
            {
                slotData[(int)ItemType.Consume].Add(Manager.Data.GameItemData.GetItem((Define.ItemType)itemType, category, count));
            }
        }

        for (int i = 0; i < slotData[(int)ItemType.Equip].Count; i++)
        {
            Equip equipItem = listEquip[i].ExtractItem();
            slotData[(int)ItemType.Equip].Add(equipItem);
        }
    }

    public void UseItem(ItemType type, int idx)
    {
        if (type == ItemType.Equip ||
            type == ItemType.Consume)
        {
            CheckData(type, idx);
            slotData[(int)type][idx]?.Used();
        }
    }

    public void SetKeyUsedType(Key keyCode)
    {
        //characterController.KeyController.ChangeInteractionRemove(keyCode);
    }

    public KeyController.KeyActionCallbackBundle GetUsedItemKeyAttachCallback(int idx)
    {
        int callIdx = idx;
        return new KeyController.KeyActionCallbackBundle(_performed: (call) => UseItem(ItemType.Consume, callIdx));
    }

    public int GetSlotCount(ItemType getType)
    {
        return getType switch
        {
            ItemType.Equip => slotData[(int)getType].Count,
            ItemType.Consume => slotData[(int)getType].Count,
            ItemType.Ect => slotData[(int)getType].Count,
            ItemType.Non => -1,
            _ => -1,
        };
    }

    public int GetItemCount(ItemType type, int idx)
    {
        CheckData(type, idx);
        return slotData[(int)type][idx].Count;
    }

    public FieldItem DropItem(ItemType type, int idx, int itemId, int count)
    {
        slotData[(int)type][idx]?.Used();

        FieldItem drop = Manager.Resource.Load<FieldItem>("Item/FieldItem");
        if (drop == null)
        {
            Message.LogError("FieldItem의 Resource Load가 실패하였습니다.");
            return null;
        }

        PooledObject pooled = Manager.Pool.GetPool(drop, characterController.transform.position, Quaternion.identity);
        if (pooled == null)
        {
            Message.LogError("FieldItem의 GetPool이 실패하였습니다.");
            return null;
        }

        drop = pooled as FieldItem;
        if (drop == null)
        {
            Message.LogError("GetPool을 통한 객체의 다운 캐스팅이 실패하였습니다.");
            return null;
        }

        drop.Init(slotData[(int)type][idx].Icon, type, itemId, count);
        return drop;
    }

    public bool PickItem(FieldItem item)
    {
        ItemType type = item.ItemType;
        int category = item.ItemId;
        int count = item.Count;

        if (int.Equals(type, ItemType.Equip) == false)
        {
            (bool findState, bool blankState ) = GetSearchOrBlankSlot(type, category, out int idx);

            if (findState)
            {
                slotData[(int)type][idx].AddItem(count);
                return true;
            }
            if (blankState &&
                GetItem(type, category, count, out Item addItem))
            {
                SetInItem(addItem, idx);
                return true;
            }
            return false;
        }


        if (GetFindBlankSlot(type, out int blankIdx) == false)
            return false;
        if (GetItem(type, category, count, out Item getItem) == false)
            return false;
        SetInItem(getItem, blankIdx);
        return true;
    }

    bool GetFindBlankSlot(ItemType type, out int blankIdx)
    {
        List<Item> searchList = slotData[(int)type];
        for (int i = 0; i < searchList.Count; i++)
        {
            if (searchList[i] == null ||
                searchList[i].Count == 0)
            {
                blankIdx = i;
                return true;
            }
        }
        blankIdx = -1;
        return false;
    }
    public void SwapSlot(ItemType type, int idx_1, int idx_2)
    {
        List<Item> itemList = slotData[(int)type];

        Item item_1     = itemList[idx_1];
        itemList[idx_1] = itemList[idx_2];
        itemList[idx_2] = item_1;
    }


    bool GetItem(ItemType itemType, int category, int count, out Item getItem)
    {
        getItem = itemType switch
        {
            ItemType.Equip => Manager.Data.GameItemData.GetItem(itemType, count, category),
            ItemType.Consume => Manager.Data.GameItemData.GetItem(itemType, count, category),
            ItemType.Ect => Manager.Data.GameItemData.GetItem(itemType, count, category),
            ItemType.Non => null,
            _ => null,
        };
        return getItem != null;
    }

    void SetInItem(Item item, int idx)
    {
        slotData[(int)item.ItemType][idx] = item;
    }

    //슬롯을 찾으면 해당 슬롯 번호 반환
    //없을 경우 -1
    int GetFindItemSlot(ItemType type, int id)
    {
        List<Item> searchList = slotData[(int)type];
        for (int i = 0; i < searchList.Count; i++)
        {
            if (searchList[i] == null)
                continue;
            if (int.Equals(searchList[i].Category, id))
                return i;
        }
        return -1;
    }

    (bool findItem, bool findBlank) GetSearchOrBlankSlot(ItemType type, int id, out int idx)
    {
        List<Item> searchList = slotData[(int)type];
        (int find, int blank) = (-1, -1);
        for (int i = 0; i < searchList.Count; i++)
        {
            if (searchList[i] == null ||
                searchList[i].Count <= 0)
            {
                if (blank < 0)
                    blank = i;
            }
            else if (int.Equals(searchList[i].Category, id))
            {
                idx = find;
                return (true, false);
            }
        }

        if (blank >= 0)
        {
            idx = blank;
            return (false, true);
        }

        idx = -1;
        return (false, false);
    }

    int RemoveItemSlot(ItemType type, int idx)
    {
        List<Item> searchList = slotData[(int)type];
        CheckData(type, idx, searchList);

        int count = searchList[idx].Count;
        slotData[(int)type][idx].MinuseItem(count);
        return count;
    }

    void MinuseItem(ItemType type, int idx, int count = -1)
    {
        List<Item> searchList = slotData[(int)type];

        CheckData(type, idx, searchList);

        searchList[idx].MinuseItem(count);
    }


    void CheckData(ItemType type, int idx, List<Item> searchList = null)
    {
        searchList ??= slotData[(int)type];

        if (searchList[idx] == null)
        {
            Message.LogWarning($"{type}의 {idx}번째 아이템은 소지중이지 않습니다. - Null Refer");
            return;
        }

        if (searchList[idx].Count <= 0)
        {
            Message.LogWarning($"{type}의 {idx}번째 아이템의 개수는 0 이하입니다. - Out Range");
            return;
        }
        if (int.Equals(searchList[idx].ItemType, type))
        {
            Message.LogWarning($"{type}의  {idx}번째 아이템의 타입은 {searchList[idx].ItemType}입니다.(삭제 시도가 이루어진 아이템입니다.) - Not Type");
            return;
        }
    }




}
