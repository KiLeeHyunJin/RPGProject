using static UserData;

public static class CharacterExtension
{
    #region Character
    public static void SetSkin(this Character character, int _hair, int _skin, int _face)
    {
        character.skin = default;
        character.skin += (ulong)(_face << (int)Define.ByteSize * (int)Define.LongSize.Six);
        character.skin += (ulong)(_skin << (int)Define.ByteSize * (int)Define.LongSize.Seven);
        character.skin += (ulong)(_hair << (int)Define.ByteSize * (int)Define.LongSize.Eight);
    }
    #endregion Character

    #region Stat
    public static void SetStat(this Stat stat, int value)
    {
        stat.str = value;
        stat.def = value;
        stat.man = value;
        stat.luk = value;
    }

    public static int GetValue(this Stat stat, Define.StatType statType)
    {
        return statType switch
        {
            Define.StatType.Str => stat.str,
            Define.StatType.Def => stat.def,
            Define.StatType.Man => stat.man,
            Define.StatType.Luk => stat.luk,
            _ => stat.str,
        };
    }

    public static void SetValue(this Stat stat, Define.StatType statType, int value)
    {
        switch (statType)
        {
            case Define.StatType.Str:
                stat.str += value;
                break;
            case Define.StatType.Def:
                stat.def += value;
                break;
            case Define.StatType.Man:
                stat.man += value;
                break;
            case Define.StatType.Luk:
                stat.luk += value;
                break;
        }
    }
    #endregion Stat

    #region Item
    public static void SaveItemData(this Item item, DecapsuleItemData itemData)
    {
        item.SetItemStat(itemData.limitStat, Define.ItemStateType.Limit);
        item.SetItemStat(itemData.addStat, Define.ItemStateType.Add);
        item.SetItemStat(itemData.upgradeStat, Define.ItemStateType.Upgrade);
        item.SetItemAility(itemData.atck, itemData.man);
        item.SetItemData(itemData.level, itemData.count, (int)itemData.type, itemData.availNum);
    }

    static void SetItemData(this UserData.Item item, int _level, int _count, int _type, int availNum)
    {
        item.itemData = default;
        item.itemData += ((ulong)_count << (int)Define.ByteSize * (int)Define.LongSize.Five);
        item.itemData += ((ulong)_count << (int)Define.ByteSize * (int)Define.LongSize.Six);
        item.itemData += ((ulong)_level << (int)Define.ByteSize * (int)Define.LongSize.Seven);
        item.itemData += ((ulong)_type << (int)Define.ByteSize * (int)Define.LongSize.Eight);
    }

    static void SetItemStat(this Item item, Stat stat, Define.ItemStateType statType)
    {
        item.SetItemStat(stat.str, stat.def, stat.man, stat.luk, statType);
    }

    static void SetItemStat(this Item item, int str, int def, int man, int luk, Define.ItemStateType statType)
    {
        SetItemStat(out ulong outStat, str, def, man, luk);
        switch (statType)
        {
            case Define.ItemStateType.Add:
                item.addStat = outStat;
                break;
            case Define.ItemStateType.Limit:
                item.limitStat = outStat;
                break;
            case Define.ItemStateType.Upgrade:
                item.upgradeStat = outStat;
                break;
        }
    }

    static void SetItemStat(out ulong stat, int str, int def, int man, int luk)
    {
        stat = default;
        stat += ((ulong)luk << (int)Define.ByteSize * (int)Define.LongSize.Five);
        stat += ((ulong)man << (int)Define.ByteSize * (int)Define.LongSize.Six);
        stat += ((ulong)def << (int)Define.ByteSize * (int)Define.LongSize.Seven);
        stat += ((ulong)str << (int)Define.ByteSize * (int)Define.LongSize.Eight);

    }

    static void SetItemAility(this Item item, int atck, int man)
    {
        item.addAbility = default;
        item.addAbility += ((ulong)man << (int)Define.ByteSize * (int)Define.LongSize.Seven);
        item.addAbility += ((ulong)atck << (int)Define.ByteSize * (int)Define.LongSize.Eight);

    }
    #endregion Item

}
