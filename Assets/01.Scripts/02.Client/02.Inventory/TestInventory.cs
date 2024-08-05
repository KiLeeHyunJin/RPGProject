using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    [SerializeField] Stat testStat;
    [SerializeField] int value;

    [SerializeField] Equip equip;
    [SerializeField] ServerData.ItemEquipServerData serverData;
    [ContextMenu("Parse")]
    public void Parse()
    {
        equip = serverData.ExtractItem();

    }
    [ContextMenu("Capsule")]
    public void Capsule()
    {
        serverData = equip.ServerEquip();

        //equip.SetEquipData
    }
}
