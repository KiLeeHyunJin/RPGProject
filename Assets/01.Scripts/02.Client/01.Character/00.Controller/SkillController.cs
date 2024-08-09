using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController
{
    readonly ScriptableSkillBundle skillBundle;
    readonly UserCharacterController characterController;


    public SkillController(UserCharacterController _characterController, ScriptableSkillBundle _skillBundle, ServerData.SkillServerData _skillData)
    {
        characterController = _characterController;
        skillBundle = _skillBundle;
    }

    public void RefreshSkillData()
    {

    }

    public void AddSkillPoint()
    {

    }

    public KeyController.KeyActionCallbackBundle GetSkillCallbackMethod()
    {
        return null;
    }

}
