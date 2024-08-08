using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버프 액티브
/// </summary>
public class ScriptableActiveBuffSkill : ScriptableBaseActiveSkill
{
    public int duringTime;
    public Stat buffStat;
    public AdditionalStat buffAdditional;
}
