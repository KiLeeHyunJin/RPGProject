using Unity.Burst.CompilerServices;
using static Define;
using static ServerData;

public partial class AbilityController
{
    readonly UserCharacterController characterController;
    readonly Ability ability;

    Stat characterStat;
    AdditionalStat characterAdditionals;

    Stat totalStats;
    AdditionalStat totalAdditionals;


    public Stat TotalStat { get { return totalStats; } }
    public AdditionalStat TotalAdditional { get { return totalAdditionals; } }


    public AbilityController(UserCharacterController owner, AbilityServerData abilityServerData)
    {
        characterController = owner;
        ability = new(abilityServerData);
    }

    public int GetCharacterPoint(CharacterPointType characterPoint)
    {
        return ability.point[(int)characterPoint];
    }

    public int GetCharacterState(CharacterStateType characterState)
    {
        return characterState switch
        {
            CharacterStateType.Hp   => ability.hp,
            CharacterStateType.Mp   => ability.mp,
            CharacterStateType.Exp  => ability.exp,
            _ => -1,
        };
    }

    public void SetAbnormalPhenomenon(float during , Stat stat, AdditionalStat additional)
    {

    }

    public void RefreshStat()
    {
        Stat wearStats = characterStat;
        AdditionalStat additionalStat = characterAdditionals;

        Equip[] wearEquips = characterController.Inventory.wearSlotData;
        foreach (Equip wearData in wearEquips)
        {
            if (wearData == null)
            {
                continue;
            }

            wearStats += wearData.BaseStat;
            wearStats += wearData.UpgradeStat;

            additionalStat += wearData.BaseAdditional;
            additionalStat += wearData.UpgradeAdditional;
        }
        totalStats = wearStats;
        totalAdditionals = additionalStat;
    }


    public void AddAbility(Define.CharacterStatType statType)
    {
        switch (statType)
        {
            case CharacterStatType.Str:
                characterStat.WarningStr = characterStat.Str + 1;
                break;
            case CharacterStatType.Def:
                characterStat.WarningDef = characterStat.Def + 1;
                break;
            case CharacterStatType.Man:
                characterStat.WarningMan = characterStat.Man + 1;
                break;
            case CharacterStatType.Luk:
                characterStat.WarningLuk = characterStat.Luk + 1;
                break;
        }
    }
}
