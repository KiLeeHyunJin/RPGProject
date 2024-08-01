using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ScriptableConsumeItem : ScriptableEctItem
{
    #region set
    public ConsumeType WarningUseType { set { useType = value; } }
    public ConsumeEffectType WarningEfxType { set { efxType = value; } }
    public int WarningValue { set { this.consumeValue = value; } }
    public int WarningDuringValue { set { duringValue = value; } }
    #endregion set

    #region get
    public ConsumeType UseType { get { return useType; } }
    public ConsumeEffectType EfxType { get { return efxType; } }
    public int Value { get { return this.consumeValue; } }
    public int DuringValue { get { return duringValue; } }
    #endregion get
    [SerializeField] private ConsumeType useType;
    [SerializeField] private ConsumeEffectType efxType;
    [SerializeField] private int consumeValue;
    [SerializeField] private int duringValue;
}

