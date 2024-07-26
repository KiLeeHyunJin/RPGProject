using System;
using static Define;

[Serializable]
public class Equip : Item
{
    public Equip(
        (int itemType, int count, int img, int scriptable) value,
        (EquipType wearType, int level, int category, int possable) _value
        ) : base(value)
    {
        EquipData(_value);
    }
    void EquipData((EquipType wearType, int level, int category, int possable) _value)
    {
        wearType = _value.wearType;
        level = _value.level;
        possableCount = _value.possable;
        category = _value.category;
    }

    public Define.EquipType wearType;

    public int level;
    public int possableCount;
    public int category;

    public Stat limitStat; //착용 제한

    public Stat baseStat; //기본 능력치
    public AdditionalStat baseAdditional;


    public Stat upgradeStat;
    public AdditionalStat upgradeAdditional;

    public Stat addAbility;
    public AdditionalStat addAdditional;

    public (int limitStat, int itemData, int baseStat, int upgradeStat, int baseAdditional, int upgradeAdditional, int addAvility) ServerEquipData()
    {
        int returnItemData = default;
        returnItemData |= ((int)wearType).Shift(DataDefine.IntSize.One);
        returnItemData |= level.Shift(DataDefine.IntSize.Two);
        returnItemData |= category.Shift(DataDefine.IntSize.Three);
        returnItemData |= possableCount.Shift(DataDefine.IntSize.Four);

        int returnLimitStat = limitStat.ServerData();
        int returnbaseStat = baseStat.ServerData();
        int returnUpgradeStat = upgradeStat.ServerData();

        int returnbaseAdditional = baseAdditional.ServerData();

        int returnUpgradeAdditional = upgradeAdditional.ServerData();
        int returnAddAvility = addAdditional.ServerData();
        return (returnLimitStat, returnItemData, returnbaseStat, returnUpgradeStat, returnbaseAdditional, returnUpgradeAdditional, returnAddAvility);
    }

    public ServerData.ItemEquipServerData ServerEquip()
    {
        ServerData.ItemEquipServerData equipData = new();
        (int limitStat, int itemData, int baseStat, int upgradeStat, int baseAdditional, int upgradeAdditional, int addAvility) = ServerEquipData();
        equipData.code = ServerItemData();
        equipData.limitStat = limitStat;
        equipData.itemData = itemData;
        equipData.baseStat = baseStat;
        equipData.upgradeStat = upgradeStat;
        equipData.baseAdditional = baseAdditional;
        equipData.upgradeAdditional = upgradeAdditional;
        equipData.addAbility = addAvility;
        return equipData;
    }
}
