using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
[Serializable]
public class Consume : Item
{
    public void ConsumeInit((HealType addType, ConsumeType efxType, int value, int stayTime) _value)
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

}
