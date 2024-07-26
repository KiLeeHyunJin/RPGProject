
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject channalPanel;
    [SerializeField] GameObject selectCharacterPanel;
    [SerializeField] GameObject creatCharacterPanel;
    NetworkRunner _runner;

    new AudioSource audio;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        audio.loop = true;
    }
    private void Start()
    {
        // NetworkRunner 인스턴스를 가져옵니다.
        _runner = FindObjectOfType<NetworkRunner>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



    private void Update()
    {
        if (_runner != null)
        {
            // 네트워크 상태를 가져옵니다.
            var connectionStatus = _runner.CurrentConnectionType;

            // 네트워크 상태를 출력합니다.
            Debug.Log($"Connection Status: {connectionStatus}");
        }
    }
    /// <summary>
    /// Fusion.NetworkRunner가 서버 또는 호스트에 성공적으로 연결되면 다시 호출합니다.
    /// </summary>
    public void OnConnectedToServer(NetworkRunner runner)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner가 서버 또는 호스트에 연결하지 못할 경우 다시 호출합니다.
    /// </summary>
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner가 원격에서 연결 요청을 수신하면 콜백
    /// </summary>
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }
    /// <summary>
    /// 인증 절차가 응답을 반환할 때 콜백이 호출됩니다
    /// </summary>
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner가 서버 또는 호스트에서 연결이 끊어지면 다시 호출합니다.
    /// </summary>
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }
    /// <summary>
    /// 호스트 마이그레이션 프로세스가 시작되었을 때 콜백이 호출됩니다
    /// </summary>
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }
    /// <summary>
    /// 사용자 입력을 폴링하는 Fusion.NetworkRunner의 콜백.Fusion.NetworkInput
    /// 제공 대상은 다음과 같습니다:
    /// input.set(새로운 CustomINetworkInput() { /* 값 */ });
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }
    /// <summary>
    /// 입력이 없을 때 Fusion.NetworkRunner에서 콜백합니다.
    /// </summary>
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner에서 새로운 Fusion.NetworkObject가 입력되었을 때 콜백
    /// </summary>
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner에서 새 Fusion.NetworkObject가 종료되면 콜백
    /// </summary>
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }
    /// <summary>
    /// 새로운 플레이어가 가입하면 Fusion.NetworkRunner에서 콜백합니다.
    /// </summary>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }
    /// <summary>
    /// 플레이어의 연결이 끊어지면 Fusion.NetworkRunner에서 콜백합니다.
    /// </summary>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }
    /// <summary>
    /// 신뢰할 수 있는 데이터 스트림을 수신하고 보고할 때 콜백이 호출됩니다
    /// </summary>
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }
    /// <summary>
    /// 신뢰할 수 있는 데이터 스트림이 수신되면 콜백이 호출됩니다
    /// </summary>
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }
    /// <summary>
    /// Scene Load가 완료되면 Callback이 호출됩니다
    /// </summary>
    /// <param name="runner"></param>
    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }
    /// <summary>
    /// Scene Load가 시작되면 Callback이 호출됩니다
    /// </summary>
    /// <param name="runner"></param>
    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
    /// <summary>
    /// Photon에서 새 세션 목록을 받을 때 호출됩니다
    /// </summary>
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }
    /// <summary>
    /// 러너가 종료될 때 호출됩니다
    /// </summary>
    /// <param name="shutdownReason">Fusion이 종료된 이유를 설명합니다</param>
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }
    /// <summary>
    /// 이 콜백은 수동으로 발송된 시뮬레이션 메시지가 수신될 때 호출됩니다
    /// </summary>
    /// <param name="runner"></param>
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }




}
