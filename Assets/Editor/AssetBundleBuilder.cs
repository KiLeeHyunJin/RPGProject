using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleBuilder
{
    [UnityEditor.MenuItem("MyTool/Cache/AllCacheClear")]
    public static void ClearCache()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        UnityWebRequest.ClearCookieCache();

        bool state = Caching.ClearCache();
        UnityEditor.EditorUtility.DisplayDialog("Cache Clear", $"캐시 정리를 시도하였습니다.\n 결과 : {state}", "완료");
    }

    [UnityEditor.MenuItem("MyTool/Bundle/Build_Bundle")]
    public static void BuildBundle()
    {
        if (!Directory.Exists(Define.dir))
            Directory.CreateDirectory(Define.dir);

        DirectoryInfo directoryInfo = new(Define.dir);

        foreach (FileInfo file in directoryInfo.GetFiles())
            file.Delete();

#if PLATFORM_STANDALONE_WIN == false

        string path = $"{Define.dir}{Define.android}";
        UnityEditor.BuildTarget buildTarget = UnityEditor.BuildTarget.Android;

#else

        string path = $"{Define.dir}{Define.window}";
        UnityEditor.BuildTarget buildTarget = UnityEditor.BuildTarget.StandaloneWindows;

#endif
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        AssetBundleManifest manifest = UnityEditor.BuildPipeline.BuildAssetBundles(
            path,
            UnityEditor.BuildAssetBundleOptions.None,
            buildTarget);
        if (manifest != null)
        {
            string[] allBundles = manifest.GetAllAssetBundles();
            WriteBundleTable(allBundles);
        }
        UnityEditor.EditorUtility.DisplayDialog("에셋 번들 빌드", "에셋 번들 빌드를 완료했습니다.", "완료");

        AssetBundle.UnloadAllAssetBundles(true);
    }

    static void WriteBundleTable(string[] allBundles)
    {
        BundleTable table = new();
        table.bundleTable = new List<BundleData>();
        for (int i = 0; i < allBundles.Length; i++)
        {
            BundleData data;
            string path = $"{Define.dir}{Define.window}/{allBundles[i]}";

            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

            data.resourceName = assetBundle.GetAllAssetNames();
            data.bundleName = assetBundle.name;
            data.bundlePath = path;

            table.bundleTable.Add(data);
        }
        File.WriteAllText($"{Define.dir}{Define.bundleTable}", JsonUtility.ToJson(table));
    }


    [UnityEditor.MenuItem("MyTool/Bundle/Refresh_BundleTable")]
    public static void ReWriteTable()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        BundleTable bundleTable = new();

        List<string> bundleList = Utils.GetAssetBundleNames();
        bundleTable.bundleTable = new List<BundleData>(bundleList.Count);

        foreach (string bundleName in bundleList)
        {
            BundleData data = new();
            string path = $"{Define.dir}{Define.window}/{bundleName}";
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            if (assetBundle == null)
                continue;
            string[] assetNames = assetBundle.GetAllAssetNames();
            data.resourceName = assetNames;
            data.bundlePath = path;
            data.bundleName = assetBundle.name;

            bundleTable.bundleTable.Add(data);
        }
        File.WriteAllText($"{Define.dir}{Define.bundleTable}", JsonUtility.ToJson(bundleTable));
        UnityEditor.EditorUtility.DisplayDialog("Refresh AssetBundleTable", "AssetBundle의 Table정보가 갱신되었습니다.", "완료");
    }

    [SerializeField]
    public struct BundleTable
    {
        public List<BundleData> bundleTable;
    }
    
    [Serializable]
    public struct BundleData
    {
        public string bundleName;
        public string bundlePath;
        public string[] resourceName;
    }

}
