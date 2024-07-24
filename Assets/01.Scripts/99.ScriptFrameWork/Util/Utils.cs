using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Utils
{

    public static void OpenFolder(string path)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                Message.LogError($"경로가 존재하지 않습니다: {path}");
                return;
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "open"
            });
        }
        catch (Exception ex)
        {
            Message.LogError($"폴더를 여는 도중 오류가 발생했습니다: {ex.Message}");
        }
    }

   



    public static List<string> GetAssetBundleNames()
    {
        string path = $"{Define.dir}/Windows";

        if (!Directory.Exists(path))
        {
            Debug.LogError("Directory does not exist: " + path);
            return new();
        }

        List<string> assetBundleNames = new();

        foreach (string filePath in Directory.GetFiles(path))
        {
            if (Path.GetExtension(filePath) == string.Empty) // Check for files without an extension
            {
                string fileName = Path.GetFileName(filePath);
                assetBundleNames.Add(fileName);
            }
        }
        return assetBundleNames;
    }

    public static void ShowInfo(string info)
    {
        PopUpUI panel = Manager.UI.ShowPopUpUI("InfoPanel");
        (panel as InfoPanel).ShowInfo(info);
    }

    public static void ShowError(System.Collections.ObjectModel.ReadOnlyCollection<Exception> exceptions, string info)
    {
        PopUpUI panel = Manager.UI.ShowPopUpUI("InfoPanel");
        (panel as InfoPanel).ShowError(exceptions, info);
    }
    public static void ShowError(System.Exception exception, string info)
    {
        PopUpUI panel = Manager.UI.ShowPopUpUI("InfoPanel");
        (panel as InfoPanel).ShowError(exception.Message, info);
    }

    /// <summary>
    /// 열거형 타입으로 변환을 해서 반환합니다.
    /// </summary>
    public static T ParseEnum<T>(string value, bool ignoreCase = true)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    /// <summary>
    /// 컴포넌트가 있으면 반환을 하고 없으면 추가를 합니다.
    /// </summary>
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        if (go == null)
            return null;
        if (!go.TryGetComponent<T>(out var component))
            component = go.AddComponent<T>();
        return component;
    }

    /// <summary>
    /// 게임오브젝트 자식중 name의 이름을 갖는 객체를 반환합니다.
    /// </summary>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
            return transform.gameObject;
        return null;
    }

    /// <summary>
    /// 게임오브젝트의 자식 중 T타입 요소의 이름이 name인 객체를 찾아 반환합니다.
    /// </summary>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            Transform transform = go.transform.Find(name);
            if (transform != null)
                return transform.GetComponent<T>();
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }



}
