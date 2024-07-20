using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject channalPanel;
    [SerializeField] GameObject selectCharacterPanel;
    [SerializeField] GameObject creatCharacterPanel;

    new AudioSource audio;
    ClientState state;

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

    public override void OnJoinedRoom()
    {
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
    }

    public override void OnLeftLobby()
    {
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Create Room Failed With Error : {returnCode} , :{message}");
    }

    public override void OnConnected()
    {
        Debug.Log("isConnect");
        PhotonNetwork.NickName = FireBaseManager.Auth.CurrentUser.DisplayName;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

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
        Utils.ShowInfo("OnDisconnected");
    }

}
