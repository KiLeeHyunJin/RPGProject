using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ScriptableConsumeItem : ScriptableEctItem
{
    #region set
    public HealType WarningUseType { set { useType = value; } }
    public ConsumeType WarningEfxType { set { efxType = value; } }
    public int WarningValue { set { this.consumeValue = value; } }
    public int WarningDuringValue { set { duringValue = value; } }
    #endregion set

    #region get
    public HealType UseType { get { return useType; } }
    public ConsumeType EfxType { get { return efxType; } }
    public int Value { get { return this.consumeValue; } }
    public int DuringValue { get { return duringValue; } }
    #endregion get
    [SerializeField] private HealType useType;
    [SerializeField] private ConsumeType efxType;
    [SerializeField] private int consumeValue;
    [SerializeField] private int duringValue;
}

