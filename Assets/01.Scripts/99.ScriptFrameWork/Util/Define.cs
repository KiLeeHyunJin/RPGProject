using System;

public class Define
{
    public readonly static int InventoryCount = 30;
    public readonly static int SlotDefaultSize = 2;

    #region Directory

    public readonly static string dir = "./Bundle";
    public readonly static string bundleTable = "/BundleTable.txt";
    public readonly static string android = "/Android";
    public readonly static string window = "/Windows";

    #endregion


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

   

}
