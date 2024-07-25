using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    public int level;

    public int type;
    public int possableCount;
    public int count;

    public Stat limitStat;

    public Stat addAbility;
    public AdditionalStat baseAdditional;

    public Stat addStat;
    public AdditionalStat addAdditional;

    public Stat upgradeStat;
    public AdditionalStat upgradeAdditional;

}
