using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Define;
using UnityEngine.UIElements;
using UnityEditor.VersionControl;
using Mono.Cecil;
using System;
using Codice.Client.BaseCommands.Merge.Xml;
using System.Drawing.Imaging;

public class CustomSkill : EditorWindow
{
    public string[] spriePath;
    public VisualElement m_RightPane;

    public JobType jobType;
    public SkillType skillType;

    public Define.CoolTimeType coolType;

    public ActiveSkillType activeSkillType;
    public PassiveSkillType passiveSkillType;

    public int limitLevel;

    public string skillName;
    public string skillInfo;








    //버프

    int duringTime;
    Stat buffStat;
    AdditionalStat buffAdditional;


    //공격
    Define.ElementType elementType;
    int attackCount;
    int targetCount;
    Vector2 Size;
    float attackPer;




    VisualElement activeField;
    VisualElement passiveField;

    public Dictionary< int,List<VisualElement>> skillDic;


    [MenuItem("Tools/Creat Skill(Scriptable)")]
    public static void ShowMyEditor()
    {
        EditorWindow wnd = GetWindow<CustomSkill>();
        wnd.titleContent = new GUIContent("Creat Skill(Scriptable)");

        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }
    public CustomSkill()
    {
        skillDic = new()
        {
            { GetKey(SkillType.Active,    (int)ActiveSkillType.Buff),          new() },
            { GetKey(SkillType.Active,    (int)ActiveSkillType.Attack),        new() },
            { GetKey(SkillType.Passive,   (int)PassiveSkillType.Probability),  new() },
            { GetKey(SkillType.Passive,   (int)PassiveSkillType.Permanent),    new() }
        };
    }

    public void CreateGUI()
    {
        // 프로젝트의 모든 스프라이트 목록 가져오기
        var allObjectGuids = AssetDatabase.FindAssets("t:Sprite", spriePath);
        var allObjects = new List<Sprite>();
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        VisualElement root = rootVisualElement;
        BaseData(root);
        ActiveAttack(root);
        ActiveBuff(root);

        Button button = new(() => { Create(); })
        {   text = "CreateSkill"  };
        root.Add(button);
        ChangeType();
    }

    void BaseData(VisualElement root)
    {
        AddEnumElement(root, "JobType",jobType, combineIgnore : true)
            .RegisterValueChangedCallback(evt =>
            { this.jobType = (JobType)evt.newValue; });


        AddTextElement(root, "SkillName", true, combineIgnore: true)
            .RegisterValueChangedCallback(evt =>
            { this.skillName = (string)evt.newValue; });

        AddIntegerElement(root, "SkillMaxLevel", true, combineIgnore: true)
            .RegisterValueChangedCallback(evt =>
            { this.limitLevel = (int)evt.newValue; });


        AddEnumElement(root, "SkillType",skillType, combineIgnore : true)
            .RegisterValueChangedCallback(evt =>
            { 
                this.skillType = (SkillType)evt.newValue;
                ChagneViewSkillType();
                ChangeType();
            });

        passiveField = AddEnumElement(root, "PassiveType", passiveSkillType, combineIgnore: true);
        (passiveField as EnumField).RegisterValueChangedCallback(evt =>
            { 
                this.passiveSkillType = (PassiveSkillType)evt.newValue;
                ChangeType();
            });

        activeField = AddEnumElement(root, "ActiveType", activeSkillType, combineIgnore: true);
        (activeField as EnumField).RegisterValueChangedCallback(evt =>
            { 
                this.activeSkillType = (ActiveSkillType)evt.newValue;
                ChangeType();
            });

        AddEnumElement(root, "CoolTimeType", coolType, combineIgnore: true)
            .RegisterValueChangedCallback(evt =>
            { this.coolType = (CoolTimeType)evt.newValue; });

        skillType = SkillType.Passive;
        ChagneViewSkillType();
    }

    void ActiveAttack(VisualElement root)
    {
        skillType = SkillType.Active;
        activeSkillType = ActiveSkillType.Attack;

        AddEnumElement(root, "ElementType", elementType)
        .RegisterValueChangedCallback(evt =>
        { this.elementType = (ElementType)evt.newValue; });

        AddIntegerElement(root, "MaxTargetCount", false, 15)
            .RegisterValueChangedCallback(evt =>
            { this.targetCount = (int)evt.newValue; });

        AddIntegerElement(root, "AttackCount", false)
            .RegisterValueChangedCallback(evt =>
            { this.attackCount = (int)evt.newValue; });

        AddFloatElement(root, "AttackSize-X", false)
            .RegisterValueChangedCallback(evt =>
            { this.Size.x = (float)evt.newValue; });

        AddFloatElement(root, "AttackSize-Y", false )
            .RegisterValueChangedCallback(evt =>
            { this.Size.y = (float)evt.newValue; });

        AddFloatElement(root, "AttackPower", false)
            .RegisterValueChangedCallback(evt =>
            { this.attackPer = (float)evt.newValue; });
    }

    void ActiveBuff(VisualElement root)
    {
        skillType = SkillType.Active;
        activeSkillType = ActiveSkillType.Buff;


    }

    void PassivePermanent(VisualElement root)
    {
        skillType = SkillType.Passive;
        passiveSkillType = PassiveSkillType.Permanent;

    }

    void PassiveProbability(VisualElement root)
    {
        skillType = SkillType.Passive;
        passiveSkillType = PassiveSkillType.Probability
            ;
    }

    IntegerField AddIntegerElement(VisualElement root, string labelName, bool display = true, int maxValue = int.MaxValue, bool combineIgnore = false)
    {
        IntegerField field = new(labelName, maxValue);
        AddVisualElement(GetItemList(), root, field, combineIgnore);

        return field;
    }
    FloatField AddFloatElement(VisualElement root, string labelName, bool display = true, int maxValue = int.MaxValue ,bool combineIgnore = false)
    {
        FloatField field = new(labelName, maxValue);
        AddVisualElement(GetItemList(), root, field, combineIgnore);

        return field;
    }

    EnumField AddEnumElement<T>(VisualElement root, string labelName, T type, bool display = true, bool combineIgnore = false) where T : System.Enum
    {
        EnumField field = new EnumField(labelName, type);
        AddVisualElement(GetItemList(), root, field, combineIgnore);

        return field;
    }

    TextField AddTextElement(VisualElement root, string labelName, bool display = true, bool? multi = null, bool combineIgnore = false)
    {
        TextField field = new TextField(labelName);

        if (multi.HasValue)
        {
            field.multiline = multi.Value;
        }
        AddVisualElement(GetItemList(), root, field, combineIgnore);
        return field;
    }



    void ChagneViewSkillType()
    {
        switch (skillType)
        {
            case SkillType.Passive:
                passiveField.style.display = DisplayStyle.Flex;
                activeField.style.display = DisplayStyle.None;
                break;
            case SkillType.Active:
                passiveField.style.display = DisplayStyle.None;
                activeField.style.display = DisplayStyle.Flex;
                break;
            case SkillType.END:
                passiveField.style.display = DisplayStyle.None;
                activeField.style.display = DisplayStyle.None;
                break;
        }
    }




    int GetKey(SkillType skillType, int detail)
    {
        return ((int)skillType << DataDefine.Int) + detail; ;
    }

    int GetDetailValue()
    {
        return skillType switch
        {
            SkillType.Passive => (int)passiveSkillType,
            SkillType.Active => (int)activeSkillType,
            SkillType.END => -1,
            _ => -1,
        };
    }



    List<VisualElement> GetItemList()
    {
        int detailType = GetDetailValue();
        int key = GetKey(skillType,detailType);
        if (skillDic.TryGetValue(key, out List<VisualElement> skillList))
            return skillList;
        return null;
    }

    void AddVisualElement(List<VisualElement> typeList, VisualElement root, VisualElement visualElement, bool combineIgnore)
    {
        root?.Add(visualElement);
        if(combineIgnore == false)
            typeList?.Add(visualElement);
    }

    void CreateActiveAttackSkill()
    {
        ScriptableActiveAttackSkill asset = ScriptableObject.CreateInstance<ScriptableActiveAttackSkill>();




        SaveAsset(asset);
    }

    void CreateActiveBuffSkill()
    {
        ScriptableActiveBuffSkill asset = ScriptableObject.CreateInstance<ScriptableActiveBuffSkill>();




        SaveAsset(asset);

    }
    void CreatePassivePermanentSkill()
    {
        ScriptablePassivePermanentSkill asset = ScriptableObject.CreateInstance<ScriptablePassivePermanentSkill>();



        SaveAsset(asset);

    }
    void CreatePassiveProbabilitySkill()
    {
        ScriptablePassiveProbabilitySkill asset = ScriptableObject.CreateInstance<ScriptablePassiveProbabilitySkill>();




        SaveAsset(asset);
    }

    void SaveAsset(UnityEngine.Object scriptable)
    {
        string path = GetPath();
        AssetDatabase.CreateAsset(scriptable, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = scriptable;
    }

    string GetPath()
    {
        string path = jobType switch
        {
            JobType.Worrior => $"{ItemScripatablePath}{Define.SkillScripatableWorriorPath}/{jobType}_{skillName}.asset",
            JobType.Archer => $"{ItemScripatablePath}{Define.SkillScripatableArcherPath}/{jobType}_{skillName}.asset",
            JobType.Wizard => $"{ItemScripatablePath}{Define.SkillScripatableWizardPath}/{jobType}_{skillName}.asset",
            _ => null,
        };
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        return AssetDatabase.GenerateUniqueAssetPath(path);
    }

    private void ChangeType()
    {
        int currentKey = GetKey(skillType, GetDetailValue());
        Debug.Log(currentKey);
        DisplayStyle state;
        foreach (var typePair in skillDic)
        {
            state = int.Equals(typePair.Key, currentKey) ? DisplayStyle.Flex : DisplayStyle.None;
            Debug.Log(typePair.Key);
            for (int i = 0; i < typePair.Value.Count; i++)
            {
                if(int.Equals(typePair.Value[i].style.display, state) == false)
                    typePair.Value[i].style.display = state;
            }
        }
    }



    void Create()
    {
        Action call = null;
        switch (skillType)
        {
            case SkillType.Passive:
                call = passiveSkillType == PassiveSkillType.Permanent ? CreatePassivePermanentSkill : CreatePassivePermanentSkill;
                break;
            case SkillType.Active:
                call = activeSkillType == ActiveSkillType.Attack ? CreateActiveAttackSkill : CreateActiveBuffSkill;
                break;
            case SkillType.END:
                return;
        }
        call?.Invoke();
    }
}
