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
    Coroutine loadVersionTableCo;
    bool isLoadServerTable;
    public int ServerCount
    {
        get
        {
            return serverVersionTable == null ? 0 :serverVersionTable.Count - 1;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        VersionRefresh((state) =>
        {
            Action action = state ? 
            LoadInit : () => 
            {
                Utils.ShowInfo("서버 데이터를 가져오지 못하였습니다."); 
            };
            action.Invoke();
        });
        serverNum = 0;
        isLoadServerTable = default;
    }

    public void VersionRefresh(Action<bool> refeshState)
    {
        if (loadVersionTableCo == null)
            loadVersionTableCo = StartCoroutine(LoadServerVersionTable(refeshState));
    }
    public void EnterServer(int _serverNum)
    {
        if (_serverNum > ServerCount)
        {
            Utils.ShowInfo($"ServerCount : {ServerCount} , Select Server Num {_serverNum}");
            return;
        }

        serverNum = _serverNum;
        SetServer();
    }

    void LoadInit()
    {
        photonServer.AppSettings.Protocol = ExitGames.Client.Photon.ConnectionProtocol.Udp;
        photonServer.AppSettings.FixedRegion = "kr";
        photonServer.DevRegion = "kr";
        isLoadServerTable = true;
        //SetServer();
    }

    

    [ContextMenu("EnterServer")]
    void SetServer()
    {
        string[] serverData = serverVersionTable[serverNum];
        photonServer.AppSettings.AppIdRealtime  = serverData[(int)ServerDataType.ServerID];
        photonServer.AppSettings.AppIdChat      = serverData[(int)ServerDataType.ChatID];
        photonServer.AppSettings.AppVersion     = serverData[(int)ServerDataType.Version];
    }



    IEnumerator LoadServerVersionTable(Action<bool> refeshState)
    {
        isLoadServerTable = default;

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
        {
            if (serverVersionTable.Capacity < length)
                serverVersionTable.Capacity = length;

            serverVersionTable.Clear();
        }
        else
        {
            serverVersionTable = new(length);
        }

        for (int i = 1; i < length; i++)
        {
            string[] versionData = rows[i].Split(',');
            serverVersionTable.Add(versionData);
        }

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
}
