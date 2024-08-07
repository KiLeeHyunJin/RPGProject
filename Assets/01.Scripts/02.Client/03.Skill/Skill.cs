using UnityEngine;
using static Define;

public abstract class Skill
{
    public SkillType SkillType { get { return skillType; } }
    SkillType skillType;
    public Sprite Icon { get { return icon; } }
    Sprite icon;

    public int MaxLevel { get { return maxLevel; } }
    int maxLevel;
    public int CurrentLevel { get { return currentLevel; } }
    int currentLevel;

    public string SkillName { get { return skillName; } }
    string skillName;
    public string SkillInfo { get { return skillInfo; } }
    string skillInfo;



}
