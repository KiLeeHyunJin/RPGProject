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
    public void SetEquipData(EquipType _wearType, int _level, Stat _limitStat, Stat _baseStat, AdditionalStat _baseAdditional)
    {
        wearType = _wearType;
        level = _level;
        limitStat = _limitStat;
        baseStat = _baseStat;
        baseAdditional = _baseAdditional;
    }
    public void InitServerData(Stat _upgradeStat, AdditionalStat _upgradeAdditional)
    {
        upgradeStat = _upgradeStat;
        upgradeAdditional = _upgradeAdditional;
    }

    int possableCount;
    public int PossableCount { get { return possableCount; } }

    Stat upgradeStat;
    public Stat UpgradeStat { get { return upgradeStat; } }
    AdditionalStat upgradeAdditional;
    public AdditionalStat UpgradeAdditional { get { return upgradeAdditional; } }


    EquipType wearType;
    public EquipType WearType { get { return wearType; } }
    int level;
    public int Level { get { return level; } }

    Stat limitStat; //착용 제한
    public Stat LimitStat { get { return limitStat; } }

    Stat baseStat; //기본 능력치
    public Stat BaseStat { get { return baseStat; } }
    AdditionalStat baseAdditional;
    public AdditionalStat BaseAdditional { get { return baseAdditional; } }

    ( int itemData, int upgradeStat, int upgradeAdditional) ServerEquipData()
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
