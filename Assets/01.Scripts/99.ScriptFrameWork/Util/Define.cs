using System;

public class Define
{
    public readonly static int SlotDefaultSize = 2;

    public readonly static int InventoryCount = 30;
    public readonly static int ItemBundleSize = 100;

    #region Directory

    public readonly static string dir = "./Bundle";
    public readonly static string android = "/Android";
    public readonly static string window = "/Windows";

    public readonly static string bundleTable = "/BundleTable.txt";

    public readonly static string ItemScripatablePath = "Assets/03.Prefabs/00.Item";
    public readonly static string ItemScripatableEctPath = "/00.Ect";
    public readonly static string ItemScripatableConsumePath = "/01.Consume";
    public readonly static string ItemScripatableEquipPath = "/02.Equip";

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

    public enum ItemType
    { 
        Equip,
        Consume,
        Ect,
        Non
    }

    public enum EquipType
    {
        Cap,
        Shirt,
        Pants,
        Shoes,
        Glove,
        Weapon
    }

    public enum ConsumeType
    {
        Heal,
        During,
    }

    public enum HealType
    {
        Hp,
        Mp,
    }



}
