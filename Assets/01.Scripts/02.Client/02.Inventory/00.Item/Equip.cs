using System;
using static Define;
using static ServerData;

[Serializable]
public class Equip : Item
{
    public void EquipInit((EquipType wearType, int level, int possable) _value)
    {
        wearType = _value.wearType;
        level = _value.level;
        possableCount = _value.possable;
    }
    public int possableCount;

    public Stat upgradeStat;
    public AdditionalStat upgradeAdditional;


    public EquipType wearType;
    public int level;

    public Stat limitStat; //착용 제한

    public Stat baseStat; //기본 능력치
    public AdditionalStat baseAdditional;

    public ( int itemData, int upgradeStat, int upgradeAdditional) ServerEquipData()
    {
        int returnItemData = default;
        returnItemData |= ((int)wearType).Shift(DataDefine.IntSize.One);
        returnItemData |= level.Shift(DataDefine.IntSize.Two);
        returnItemData |= possableCount.Shift(DataDefine.IntSize.Three);

        int returnUpgradeStat = upgradeStat.ServerData();
        int returnUpgradeAdditional = upgradeAdditional.ServerData();

        return (returnItemData,  returnUpgradeStat, returnUpgradeAdditional);
    }

    public ItemEquipServerData ServerEquip()
    {
        ItemEquipServerData equipData = new();
        (int itemData, int upgradeStat, int upgradeAdditional) = ServerEquipData();

        equipData.code = ServerItemData();

        equipData.itemData = itemData;
        equipData.upgradeStat = upgradeStat;
        equipData.upgradeAdditional = upgradeAdditional;
        return equipData;
    }
}
