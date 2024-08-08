using static Define;
using static ServerData;

public partial class AbilityController
{
    readonly UserCharacterController characterController;

    Stat[] stats;
    AdditionalStat[] additionals;

    int[] points;
    int[] state;
    public AbilityController(UserCharacterController owner, AbilityServerData abilityServerData)
    {
        characterController = owner;

        points = abilityServerData.ParsPoint();
        state = new int[] { abilityServerData.hp, abilityServerData.mp, abilityServerData.exp };
        stats = new Stat[] { new(abilityServerData.ParseStat()) };
    }

    public int GetCharacterPoint(CharacterPointType characterPoint)
    {
        return points[(int)characterPoint];
    }

    public int GetCharacterState(CharacterStateType characterState)
    {
        return state[(int)characterState];
    }

    public void UpdateAbilityValue()
    {

    }
}
