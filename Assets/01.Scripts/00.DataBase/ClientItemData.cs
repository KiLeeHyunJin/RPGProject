using System;

[Serializable]
public class ClientItemData
{
    public string itemName;
    public int level;
    public int count;
    public int availNum;
    public Define.ItemType type;

    public int atck;
    public int man;

    public UserData.Stat limitStat;
    public UserData.Stat addStat;
    public UserData.Stat upgradeStat;

    public void ParseItemData(UserData.Item capsuleItem)
    {
        itemName = capsuleItem.itemName;
        ulong itemData = capsuleItem.itemData;

        int _type = (int)((itemData << (int)Define.ByteSize * (int)Define.LongSize.One) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        level = (int)((itemData << (int)Define.ByteSize * (int)Define.LongSize.Two) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        count = (int)((itemData << (int)Define.ByteSize * (int)Define.LongSize.Three) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        availNum = (int)((itemData << (int)Define.ByteSize * (int)Define.LongSize.Four) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        type = (Define.ItemType)_type;

        ParseStat(ref addStat,      capsuleItem.addStat);
        ParseStat(ref limitStat,    capsuleItem.limitStat);
        ParseStat(ref upgradeStat,  capsuleItem.upgradeStat);

        #region
        //ulong _addStat = capsuleItem.addStat;

        //addStat.str = (int)((_addStat << (int)Define.ByteSize * (int)Define.LongSize.One) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        //addStat.def = (int)((_addStat << (int)Define.ByteSize * (int)Define.LongSize.Two) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        //addStat.man = (int)((_addStat << (int)Define.ByteSize * (int)Define.LongSize.Three) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        //addStat.luk = (int)((_addStat << (int)Define.ByteSize * (int)Define.LongSize.Four) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);

        //ulong _limitStat = capsuleItem.limitStat;

        //limitStat.str = (int)((_limitStat << (int)Define.ByteSize * (int)Define.LongSize.One) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        //limitStat.def = (int)((_limitStat << (int)Define.ByteSize * (int)Define.LongSize.Two) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        //limitStat.man = (int)((_limitStat << (int)Define.ByteSize * (int)Define.LongSize.Three) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        //limitStat.luk = (int)((_limitStat << (int)Define.ByteSize * (int)Define.LongSize.Four) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        #endregion

        ulong _addAbility = capsuleItem.addAbility;

        atck = (int)((_addAbility << (int)Define.ByteSize * (int)Define.LongSize.One) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        man  = (int)((_addAbility << (int)Define.ByteSize * (int)Define.LongSize.Two) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);

    }

    void ParseStat(ref UserData.Stat stat, ulong dataLong)
    {
        stat.str = (int)((dataLong << (int)Define.ByteSize * (int)Define.LongSize.One) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        stat.def = (int)((dataLong << (int)Define.ByteSize * (int)Define.LongSize.Two) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        stat.man = (int)((dataLong << (int)Define.ByteSize * (int)Define.LongSize.Three) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
        stat.luk = (int)((dataLong << (int)Define.ByteSize * (int)Define.LongSize.Four) >> (int)Define.ByteSize * (int)Define.LongSize.Eight);
    }
}

