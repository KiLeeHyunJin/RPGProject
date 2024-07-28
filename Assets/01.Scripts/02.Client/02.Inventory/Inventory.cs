using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Inventory
{
    public List<List<Item>> slotData;
    public int[] slotCounts;

    public Inventory(int equipCount, int consumeCount, int ectCount)
    {
        slotCounts = new int[] { equipCount, consumeCount, ectCount };
        slotData = new((int)ItemType.Non);
        for (int i = 0; i < slotData.Capacity; i++)
            slotData[i] = new(slotCounts[i]);
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
