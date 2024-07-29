using System;
using static Define;
[Serializable]
public class Consume : Item
{
    public void SetConsumeData(HealType _useType, ConsumeType _efxType, int _value, int _during)
    {
        useType = _useType;
        efxType = _efxType;
        value = _value;
        duringValue = _during;
    }
    public HealType useType;
    public ConsumeType efxType;
    public int value;
    public int duringValue;

}
