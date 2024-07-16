using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    //public enum Panel { Login, Menu, Lobby}

    [SerializeField] GameObject loginPanel;

    new AudioSource audio;
    private ClientState state;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        audio.loop = true;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Update()
    {
        ClientState currentState = PhotonNetwork.NetworkClientState;
        if (state == currentState)
            return;
        state = currentState;
        Debug.Log(currentState);
    }

    //private void SetActivePanel(Panel panel)
    //{
    //    loginPanel.gameObject?.SetActive(panel == Panel.Login);
    //}

    public override void OnJoinedRoom()
    {
       // if(data.GetLobbyState(LobbyData.LobbyState.Random) == false)
        Debug.Log($"Join Room Success");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"Create Room Success");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("leftRoom");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Random Room Failed with Error : {returnCode}, {message}");
    }
    public override void OnJoinedLobby()
    {
        //SetActivePanel(Panel.Lobby);
    }

    public override void OnLeftLobby()
    {
        //SetActivePanel(Panel.Menu);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Create Room Failed With Error : {returnCode} , :{message}");
    }

    public override void OnConnected()
    {
        Debug.Log("isConnect");
        PhotonNetwork.NickName = FireBaseManager.Auth.CurrentUser.DisplayName;
        
        //SetActivePanel(Panel.Menu);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //if (data.GetLobbyState(LobbyData.LobbyState.Random) == false) //랜덤 매칭이 아닐 경우 로비 업데이트
        //    lobbyPanel.UpdateRoomList(roomList);
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause == DisconnectCause.DisconnectByServerLogic)
            return;
        //SetActivePanel(Panel.Login);
        Debug.Log("OnDisconnected");
    }

}
