using System;
[Serializable]
public struct Stat
{
    public Stat(int _str, int _def, int _man, int _luk)
    {
        str = _str;
        def = _def;
        man = _man;
        luk = _luk;
    }
    public int def;
    public int luk;
    public int man;
    public int str;
    public int ServerData()
    {
        int returnValue = default;
        returnValue |= str.Shift(DataDefine.IntSize.One);
        returnValue |= def.Shift(DataDefine.IntSize.Two);
        returnValue |= man.Shift(DataDefine.IntSize.Three);
        returnValue |= luk.Shift(DataDefine.IntSize.Four);
        return returnValue;
    }
}
