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
                Message.LogError($"��ΰ� �������� �ʽ��ϴ�: {path}");
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
            Message.LogError($"������ ���� ���� ������ �߻��߽��ϴ�: {ex.Message}");
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
    /// ������ Ÿ������ ��ȯ�� �ؼ� ��ȯ�մϴ�.
    /// </summary>
    public static T ParseEnum<T>(string value, bool ignoreCase = true)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    /// <summary>
    /// ������Ʈ�� ������ ��ȯ�� �ϰ� ������ �߰��� �մϴ�.
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
    /// ���ӿ�����Ʈ �ڽ��� name�� �̸��� ���� ��ü�� ��ȯ�մϴ�.
    /// </summary>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
            return transform.gameObject;
        return null;
    }

    /// <summary>
    /// ���ӿ�����Ʈ�� �ڽ� �� TŸ�� ����� �̸��� name�� ��ü�� ã�� ��ȯ�մϴ�.
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
