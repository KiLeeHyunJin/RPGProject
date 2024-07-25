using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<List<Item>> slotData;

    public int[] slotCounts;

    public Inventory(int equipCount, int consumeCount, int ectCount)
    {
        slotCounts = new int[] { equipCount, consumeCount, ectCount };
        slotData = new(3);
        for (int i = 0; i < slotData.Capacity; i++)
        {
            slotData[i] = new(slotCounts[i]);
        }
    }


    public bool AddItem(int idx)
    {
        if (slotData.Count <= slotCounts[idx])
            return false;

        return true;
    }

    public void RemoveItem()
    {

    }

}
