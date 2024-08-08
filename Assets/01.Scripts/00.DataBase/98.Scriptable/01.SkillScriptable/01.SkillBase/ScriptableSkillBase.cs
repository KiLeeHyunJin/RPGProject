using UnityEngine;

public abstract class ScriptableSkillBase : ScriptableObject
{
    public string skillName;
    public string skillInfo;

    public int limitLevel;

    public Define.SkillType skillType;
    public Define.CoolTimeType coolType;
}
