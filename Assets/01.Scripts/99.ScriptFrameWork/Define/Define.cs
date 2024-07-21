using System;

public class Define
{
    public readonly static int InventoryCount = 30;

    public readonly static string dir = "./Bundle";
    public readonly static string bundleTable = "/BundleTable.txt";
    public readonly static string android = "/Android";
    public readonly static string window = "/Windows";

    public readonly static string SearchNickName = "nickNames";
    public readonly static string UseNickName = "useName";
    public readonly static string User = "users";
    public readonly static string UserId = "userId";
    public readonly static string Character = "characters";
    public readonly static string Ability = "ability";
    public readonly static string Inventory = "inventory";
    public readonly static string Job = "job";
    public readonly static string Level = "level";
    public readonly static string NickName = "nickName";
    public readonly static string Skill = "skill";
    public readonly static string Stat = "stat";

    public readonly static int ByteSize = 8;
    public readonly static int SlotDefaultSize = 2;
    //마지막 배열을 요소를 가리킨다
    public static Index EndIndex = ^1;

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
    }

    public enum Font
    {
        MBold,
        MLight,
    }

    public enum StatType
    {
        Str,
        Def,
        Man,
        Luk,
    }

    public enum ItemType
    {
        Ect,
        Consume,
        Equip,
    }

    public enum ItemStateType
    {
        Add,
        Limit,
        Upgrade,
    }

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
}
