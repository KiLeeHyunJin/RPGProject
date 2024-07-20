using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<string, Object> resources = new();

    public T Load<T>(string path) where T : Object
    {
        string key = $"{path}_{typeof(T)}";

        if (resources.TryGetValue(key, out Object obj))
        {
            return obj as T;
        }
        else
        {
            T resource = Resources.Load<T>(path);
            if (resource == null)
                return null;

            resources.Add(key, resource);
            return resource;
        }
    }
}
