using System;
using UnityEngine;

public class Define
{
    public readonly static Vector2 S20 = new(720, 1544);
    public readonly static int InventoryCount = 30;

    public readonly static string[] ignoreRefName = { "UISprite", "UIMask", "Background", "BackGround", "Scroll", "Circle" };

    public readonly static string workDir = "00.WorkSpace/";
    public readonly static string hshWorkDir = "00.HSH/Prefabs/";
    public readonly static string kmsWorkDir = "00.KMS/Prefabs/";
    public readonly static string hjuWorkDir = "00.HJU/Prefabs/";

    public readonly static string dir = "./Bundle";
    public readonly static string bundleTable = "/BundleTable.txt";
    public readonly static string android = "/Android";
    public readonly static string window = "/Windows";


    //마지막 배열을 요소를 가리킨다
    public static Index EndIndex = ^1;

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
    }


    public enum Sound
    {
    }

    public enum Font
    { 
        MBold,
        MLight,


    }


}
