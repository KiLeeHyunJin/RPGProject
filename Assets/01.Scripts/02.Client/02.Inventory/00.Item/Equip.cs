using System;
using UnityEngine;
using static Define;
using static ServerData;

[Serializable]
public class Equip : Item
{
    public int PossableCount { get { return possableCount; } }
    int possableCount;
    public Stat UpgradeStat { get { return upgradeStat; } }
    Stat upgradeStat;
    public AdditionalStat UpgradeAdditional { get { return upgradeAdditional; } }
    AdditionalStat upgradeAdditional;
    public EquipType WearType { get { return wearType; } }
    EquipType wearType;
    public int Level { get { return level; } }
    int level;
    public Stat LimitStat { get { return limitStat; } }
    Stat limitStat; //착용 제한
    public Stat BaseStat { get { return baseStat; } }
    Stat baseStat; //기본 능력치
    public AdditionalStat BaseAdditional { get { return baseAdditional; } }
    AdditionalStat baseAdditional;

    public Equip(
        out Action<(int itemType, int count, int category)> _Init, 
        out Action<string, string, int, Sprite, int> _SetEctData, 
        out Action<EquipType, int, Stat, Stat, AdditionalStat> _SetEquip,
        out Action<int, Stat, AdditionalStat> _InitServerData) : base(out _Init, out _SetEctData)
    {
        _SetEquip = SetEquipData;
        _InitServerData = InitServerData;
    }


    #region EquipInit

    void SetEquipData(EquipType _wearType, int _level, Stat _limitStat, Stat _baseStat, AdditionalStat _baseAdditional)
    {
        wearType = _wearType;
        level = _level;
        limitStat = _limitStat;
        baseStat = _baseStat;
        baseAdditional = _baseAdditional;
    }

    void InitServerData(int possable, Stat _upgradeStat, AdditionalStat _upgradeAdditional)
    {
        upgradeStat = _upgradeStat;
        upgradeAdditional = _upgradeAdditional;
        possableCount = possable;
    }
    #endregion



    #region EquipCapsule
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

    (int itemData, int upgradeStat, int upgradeAdditional) ServerEquipData()
    {
        int returnItemData = default;
        returnItemData |= possableCount.Shift(DataDefine.IntSize.One);

        int returnUpgradeStat = upgradeStat.ServerData();
        int returnUpgradeAdditional = upgradeAdditional.ServerData();

        return (returnItemData, returnUpgradeStat, returnUpgradeAdditional);
    }
    #endregion EquipCapsule






}
