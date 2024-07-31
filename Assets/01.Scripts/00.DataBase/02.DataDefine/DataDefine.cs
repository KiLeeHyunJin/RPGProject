using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefine
{
    public readonly static string SearchNickName = "nickNames";
    public readonly static string UseNickName = "useName";
    public readonly static string NickName = "nickName";


    public readonly static string User = "users";
    public readonly static string UserId = "userId";
    public readonly static string Character = "characters";
    public readonly static string Ability = "ability";
    public readonly static string Inventory = "inventory";
    public readonly static string Job = "job";
    public readonly static string Level = "level";
    public readonly static string Skill = "skill";
    public readonly static string Stat = "stat";



    public readonly static int Byte = 8;

    public readonly static int Long = 8;
    public readonly static int Int = 4;
    public readonly static int Short = 2;


    public enum LongSize
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
    }

    public enum IntSize
    {
        One,
        Two,
        Three,
        Four,
    }

    public enum ShortSize
    {
        One,
        Two,
    }
}
