using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerDataManager : Singleton<ServerDataManager>
{
    const string DownloadPath = "https://drive.google.com/uc?export=download&id=";
    const string serverVersionTableURL = "https://docs.google.com/spreadsheets/d/1um5DzNP_nYsw0XQ8v8vWW9kjLK5fYv8WOxlOLx9KO0U/export?format=csv";  // 서버 id

    const string localVersionTablePath = "/versionTable.csv";
    readonly string localVersionPath = Application.streamingAssetsPath + "/AssetBundles";

    [SerializeField] ServerSettings photonServer;
    [SerializeField] List<string[]> serverVersionTable;
    [SerializeField] int serverNum;

    protected override void Awake()
    {
        base.Awake();
        VersionRefresh((state) =>
        {
            Action action = state ? LoadInit : LoadError;
            action.Invoke();
        });
        serverNum = 0;
    }

    void LoadError()
    {
        Debug.Log("서버 데이터를 가져오지 못하였습니다.");
    }

    void LoadInit()
    {
        photonServer.AppSettings.Protocol = ExitGames.Client.Photon.ConnectionProtocol.Udp;
        photonServer.AppSettings.FixedRegion = "kr";
        photonServer.DevRegion = "kr";
        SetServer();
    }

    [ContextMenu("EnterServer")]
    void SetServer()
    {
        string[] serverData = serverVersionTable[serverNum];
        photonServer.AppSettings.AppIdRealtime  = serverData[(int)ServerDataType.ServerID];
        photonServer.AppSettings.AppIdChat      = serverData[(int)ServerDataType.ChatID];
        photonServer.AppSettings.AppVersion     = serverData[(int)ServerDataType.Version];
    }

    public void VersionRefresh(Action<bool> refeshState)
    {
        StartCoroutine(LoadServerVersionTable(refeshState));
    }

    IEnumerator LoadServerVersionTable(Action<bool> refeshState)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            refeshState?.Invoke(false);
            yield break;
        }

        using UnityWebRequest uwr = UnityWebRequest.Get(serverVersionTableURL);
        yield return uwr.SendWebRequest();

        string[] rows = uwr.downloadHandler.text.Split('\n');
        int length = rows.Length;

        if (serverVersionTable != null)
            serverVersionTable.Clear();
        else
            serverVersionTable = new(length);

        for (int i = 1; i < length; i++)
        {
            string[] versionData = rows[i].Split(',');
            serverVersionTable.Add(versionData);
        }

        //if (System.IO.Directory.Exists(localVersionPath) == false)
        //    System.IO.Directory.CreateDirectory(localVersionPath);

        //System.IO.FileStream fs = new($"{localVersionPath}{localVersionTablePath}", System.IO.FileMode.Create);

        //byte[] data = uwr.downloadHandler.data;
        //fs.Write(data, 0, data.Length);
        //fs.Dispose();

        refeshState?.Invoke(serverVersionTable != null);
    }



    string ExtractSubstring(string inputString)
    {
        const string startMarker = "d/";
        const string endMarker = "/view";

        int startIndex = inputString.IndexOf(startMarker);
        if (startIndex == -1)
            return null;
        startIndex += startMarker.Length;
        int endIndex = inputString.IndexOf(endMarker, startIndex);
        if (endIndex == -1)
            return null;

        return $"{DownloadPath}{inputString[startIndex..endIndex]}";
    }

    enum ServerDataType
    { 
        Name,
        Version,
        ServerID,
        ChatID,
    }

    enum VersionTable// 버전 테이블 Column(열) 정보
    {
        ServerName, // 번들 파일 명
        Version, // 번들 버전 정보
        PunId, // 번들 설치 링크
        ChatId, // 번들 설치 링크
    }
}
