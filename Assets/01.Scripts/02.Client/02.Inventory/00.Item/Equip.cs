using static Define;

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

    public Stat addAbility; 
    public AdditionalStat addAdditional;

    public Stat upgradeStat;
    public AdditionalStat upgradeAdditional;

    public (int limitStat, long itemData, long addStat, long upgradeStat) ServerEquipData()
    {
        int limitStat = default;
        long itemData = default;
        long addStat = default;
        long upgradeStat = default;
        return (limitStat, itemData, addStat, upgradeStat);
    }
}
