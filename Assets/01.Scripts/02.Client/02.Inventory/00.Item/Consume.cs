using System;
using UnityEngine;
using static Define;
[Serializable]
public  class Consume : Item, IKeyAttachable
{
    public ConsumeType UseType { get { return useType; } }
    ConsumeType useType;
    public ConsumeEffectType EfxType { get { return efxType; } }
    ConsumeEffectType efxType;
    public int Value { get { return value; } }
    int value;

    public int DuringValue { get { return duringValue; } }

    int duringValue;

    public Consume(
        out Action<(int itemType, int count, int category)> _Init, 
        out Action<string, string, int, Sprite, int> _SetEctData,
        out Action<ConsumeType, ConsumeEffectType, int , int > _SetConsumeData) : base(out _Init, out _SetEctData)
    {
        _SetConsumeData = SetConsumeData;
    }


    void SetConsumeData(ConsumeType _useType, ConsumeEffectType _efxType, int _value, int _during)
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
