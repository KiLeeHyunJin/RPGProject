public partial class AbilityController
{
    Stat[] stats;
    AdditionalStat[] additionals;
    UserCharacterController characterController;
    public AbilityController(UserCharacterController owner)
    {
        characterController = owner;
    }

    public void UpdateAbilityValue()
    {

    }
}
