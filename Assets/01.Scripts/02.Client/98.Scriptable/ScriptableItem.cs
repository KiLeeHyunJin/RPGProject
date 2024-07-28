using UnityEngine;
using static Define;

public class ScriptableItem : MonoBehaviour { }

public class ScriptableEctItem : ScriptableObject
{
    #region set
    public string WarningItemName { set { itemName = value; } }
    public string WarningItemInfo { set { itemInfo = value; } }
    public ItemType WarningItemType { set { itemType = value; } }
    public int WarningCount { set { count = value; } }
    public int WarningImgData { set { imgData = value; } }
    public int WarningScriptableData { set { scriptableData = value; } }
    public int WarningPrice { set { price = value; } }
    public int WaringCategory { set { category = value; } }

    #endregion set

    #region get

    public string ItemName { get { return itemName; } }
    public string ItemInfo { get { return itemInfo; } }
    public ItemType ItemType { get { return itemType; } }
    public int Count { get { return count; } }
    public int ImgData { get { return imgData; } }
    public int ScriptableData { get { return scriptableData; } }
    public int Price { get { return price; } }
    public int Category { get { return category; } }

    #endregion get

    [SerializeField] private int category;

    [SerializeField]private string itemName;
    [SerializeField]private string itemInfo;
    [SerializeField]private ItemType itemType;
    [SerializeField]private int count;
    [SerializeField]private int imgData;
    [SerializeField]private int scriptableData;
    [SerializeField] private int price;
}

public class ScriptableConsumeItem : ScriptableEctItem
{
    #region set
    public HealType WarningUseType { set { useType = value; } }
    public ConsumeType WarningEfxType { set { efxType = value; } }
    public int WarningValue { set { this.consumeValue = value; } }
    public int WarningDuringValue { set { duringValue = value; } }
    #endregion set

    #region get
    public HealType UseType {  get { return useType; } }
    public ConsumeType EfxType {  get { return efxType; } }
    public int Value {  get { return this.consumeValue; } }
    public int DuringValue {  get { return duringValue; } }
    #endregion get
    [SerializeField]private HealType useType;
    [SerializeField]private ConsumeType efxType;
    [SerializeField]private int consumeValue;
    [SerializeField]private int duringValue;
}

public class ScriptableEquipItem : ScriptableEctItem
{
    #region set
    public EquipType WarningWearType { set { wearType = value; } }
    public int WarningLevel { set { level = value; } }
    public int WarningPossableCount { set { possableCount = value; } }
    public Stat WarningLimitStat { set { limitStat = value; } }
    public Stat WarningBaseStat { set { baseStat = value; } }
    public Stat WarningUpgradeStat { set { upgradeStat = value; } }
    public AdditionalStat WarningBaseAdditional { set { baseAdditional = value; } }
    public AdditionalStat WarningUpgradeAdditional { set { upgradeAdditional = value; } }
    #endregion set

    #region get
    public EquipType WearType {  get { return wearType; } }
    public int Level {  get { return level; } }
    public int PossableCount {  get { return possableCount; } }
    public Stat LimitStat {  get { return limitStat; } }
    public Stat BaseStat {  get { return baseStat; } }
    public Stat UpgradeStat {  get { return upgradeStat; } }
    public AdditionalStat BaseAdditional {  get { return baseAdditional; } }
    public AdditionalStat UpgradeAdditional {  get { return upgradeAdditional; } }
    #endregion get

    [SerializeField]private  EquipType wearType;
    [SerializeField]private  int level;
    [SerializeField]private  int possableCount;
    [SerializeField]private  Stat limitStat; //착용 제한
    [SerializeField]private  Stat baseStat; //기본 능력치
    [SerializeField]private  AdditionalStat baseAdditional;
    [SerializeField]private  Stat upgradeStat;
    [SerializeField] private AdditionalStat upgradeAdditional;
}

