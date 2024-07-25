using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : Item
{
    public int level;
    public int possableCount;

    public Stat limitStat;

    public Stat addAbility;
    public AdditionalStat baseAdditional;

    public Stat addStat;
    public AdditionalStat addAdditional;

    public Stat upgradeStat;
    public AdditionalStat upgradeAdditional;
}
