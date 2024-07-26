using System;

[Serializable]
public class AdditionalStat
{
    //힘, 민첩, 지력, 운, 공격, 마법, 방어, 이속, 
    public AdditionalStat(int _atck, int _magic, int _defence, int _speed)
    {
        power = _atck;
        magic = _magic;
        defence = _defence;
        speed = _speed;
    }
    public int magic;
    public int power;
    public int defence;
    public int speed;

    public int ServerData()
    {
        int returnValue = default;
        returnValue |= magic.Shift(DataDefine.IntSize.One);
        returnValue |= power.Shift(DataDefine.IntSize.Two);
        returnValue |= defence.Shift(DataDefine.IntSize.Three);
        returnValue |= speed.Shift(DataDefine.IntSize.Four);
        return returnValue;
    }
}
