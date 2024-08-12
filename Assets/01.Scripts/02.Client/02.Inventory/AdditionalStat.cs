using System;

[Serializable]
public struct AdditionalStat
{
    public int WarningMagic { set { magic = value; } }
    public int Magic { get { return magic; } }
    int magic;
    public int WarningPower { set { power = value; } }
    public int Power { get { return power; } }
    int power;
    public int WarningDefence { set { defence = value; } }
    public int Defence { get { return defence; } }
    int defence;
    public int WarningSpeed { set { speed = value; } }
    public int Speed { get { return speed; } }
    int speed;
    //힘, 민첩, 지력, 운, 공격, 마법, 방어, 이속, 
    public AdditionalStat(int _atck, int _magic, int _defence, int _speed)
    {
        power = _atck;
        magic = _magic;
        defence = _defence;
        speed = _speed;
    }
    public int ServerData()
    {
        int returnValue = default;
        returnValue |= magic.Shift(DataDefine.IntSize.One);
        returnValue |= power.Shift(DataDefine.IntSize.Two);
        returnValue |= defence.Shift(DataDefine.IntSize.Three);
        returnValue |= speed.Shift(DataDefine.IntSize.Four);
        return returnValue;
    }

    public static AdditionalStat operator +(AdditionalStat inValue, AdditionalStat currentValue)
    {
        return new AdditionalStat(
            inValue.power + currentValue.power,
            inValue.magic + currentValue.magic,
            inValue.defence + currentValue.defence,
            inValue.speed + currentValue.speed);
    }
}
