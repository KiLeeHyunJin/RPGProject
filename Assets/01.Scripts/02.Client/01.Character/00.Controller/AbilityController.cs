using System;
using UnityEngine;
using static Define;
using static ServerData;
[Serializable]
public class AbilityController
{
    readonly UserCharacterController characterController;

    [SerializeField] Stat[] stats;
    [SerializeField] AdditionalStat[] additionals;

    [SerializeField] int[] points;
    [SerializeField] int[] state;

    public AbilityController(UserCharacterController owner, AbilityServerData abilityServerData)
    {
        characterController = owner;

        points = abilityServerData.ParsPoint();
        stats = new Stat[] { new(abilityServerData.ParseStat()) };
        state = new int[] { abilityServerData.hp, abilityServerData.mp, abilityServerData.exp };
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
