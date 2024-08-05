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
    public int WarningDef { set { str = value; } }
    public int Def { get { return str; } }
    int def;
    public int WarningLuk { set { str = value; } }
    public int Luk { get { return str; } }
    int luk;
    public int WarningMan { set { str = value; } }
    public int Man { get { return str; } }
    int man;
    public int WarningStr { set { str = value; } }
    public int Str { get { return str; } }
    int str;
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
