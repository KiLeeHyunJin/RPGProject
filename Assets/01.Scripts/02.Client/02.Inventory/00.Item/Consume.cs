using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
[Serializable]
public class Consume : Item
{
    public Consume(
        (int itemType, int count, int img, int scriptable) value,
        (HealType addType, ConsumeType efxType, int value, int stayTime) _value) : base(value)
    {
        ConsumeData(_value);
    }

    void ConsumeData((HealType addType, ConsumeType efxType, int value, int stayTime) _value)
    {
        useType = _value.addType;
        efxType = _value.efxType;
        value = _value.value;
        duringValue = _value.stayTime;
    }

    public HealType useType;
    public ConsumeType efxType;
    public int value;
    public int duringValue;

    public long SeverConsumeData()
    {
        long returnValue = default;
        returnValue += ((long)useType).Shift(DataDefine.LongSize.One);
        returnValue += ((long)efxType).Shift(DataDefine.LongSize.Two);
        returnValue += ((long)value).Shift(DataDefine.LongSize.Three);
        returnValue += ((long)duringValue).Shift(DataDefine.LongSize.Four);
        return returnValue;
    }
}
