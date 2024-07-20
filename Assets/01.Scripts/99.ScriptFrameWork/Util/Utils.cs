using System;
using UnityEngine;

public class Utils
{

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


    public static T ParseEnum<T>(string value, bool ignoreCase = true)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        if (go == null)
            return null;
        if (!go.TryGetComponent<T>(out var component))
            component = go.AddComponent<T>();
        return component;
    }

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

    public static T FindChild<T>(Transform parent, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (parent == null)
            return null;

        if (recursive == false)
        {
            Transform transform = parent.Find(name);
            if (transform != null)
                return transform.GetComponent<T>();
        }
        else
        {
            foreach (Transform child in parent)
            {
                if (child.TryGetComponent<T>(out T component))
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
            return transform.gameObject;
        return null;
    }

}
