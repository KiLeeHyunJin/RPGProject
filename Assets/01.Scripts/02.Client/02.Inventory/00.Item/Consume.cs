using System;
using static Define;
[Serializable]
public class Consume : Item
{
    public void SetConsumeData(ConsumeType _useType, ConsumeEffectType _efxType, int _value, int _during)
    {
        useType = _useType;
        efxType = _efxType;
        value = _value;
        duringValue = _during;
    }
    public ConsumeType useType;
    public ConsumeEffectType efxType;
    public int value;
    public int duringValue;

}
