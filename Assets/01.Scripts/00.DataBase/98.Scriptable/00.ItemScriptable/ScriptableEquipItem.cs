using UnityEngine;
using static Define;


public class ScriptableEquipItem : ScriptableEctItem
{
    #region set
    public EquipType WarningWearType { set { wearType = value; } }
    public int WarningLevel { set { level = value; } }
    public int WarningPossableCount { set { possableCount = value; } }
    public Stat WarningLimitStat { set { limitStat = value; } }
    public Stat WarningBaseStat { set { baseStat = value; } }
    public AdditionalStat WarningBaseAdditional { set { baseAdditional = value; } }

    //public AdditionalStat WarningUpgradeAdditional { set { upgradeAdditional = value; } }
    //public Stat WarningUpgradeStat { set { upgradeStat = value; } }

    #endregion set

    #region get
    public EquipType WearType { get { return wearType; } }
    public int Level { get { return level; } }
    public int PossableCount { get { return possableCount; } }
    public Stat LimitStat { get { return limitStat; } }
    public Stat BaseStat { get { return baseStat; } }
    public AdditionalStat BaseAdditional { get { return baseAdditional; } }

    //public AdditionalStat UpgradeAdditional { get { return upgradeAdditional; } }
    //public Stat UpgradeStat { get { return upgradeStat; } }

    #endregion get

    [SerializeField] private EquipType wearType;
    [SerializeField] private int level;
    [SerializeField] private int possableCount;
    [SerializeField] private Stat limitStat; //착용 제한
    [SerializeField] private Stat baseStat; //기본 능력치
    [SerializeField] private AdditionalStat baseAdditional;
    //[SerializeField] private Stat upgradeStat;
    //[SerializeField] private AdditionalStat upgradeAdditional;
}
