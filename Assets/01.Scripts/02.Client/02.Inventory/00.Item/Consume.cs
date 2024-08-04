using System;
using static Define;
[Serializable]
public class Consume : Item, IKeyAttachable
{
    ConsumeType useType;
    public ConsumeType UseType { get { return useType; } }
    ConsumeEffectType efxType;
    public ConsumeEffectType EfxType { get { return efxType; } }
    int value;
    public int Value { get { return value; } }
    int duringValue;
    public int DuringValue { get { return duringValue; } }


    public void SetConsumeData(ConsumeType _useType, ConsumeEffectType _efxType, int _value, int _during)
    {
        useType = _useType;
        efxType = _efxType;
        value = _value;
        duringValue = _during;
    }

    public override void Used()
    {

    }

    public void Started()
    {

    }

    public void Performed()
    {

    }

    public void Canceled()
    {

    }



}
