
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
        // NetworkRunner �ν��Ͻ��� �����ɴϴ�.
        _runner = FindObjectOfType<NetworkRunner>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



    private void Update()
    {
        if (_runner != null)
        {
            // ��Ʈ��ũ ���¸� �����ɴϴ�.
            var connectionStatus = _runner.CurrentConnectionType;

            // ��Ʈ��ũ ���¸� ����մϴ�.
            Debug.Log($"Connection Status: {connectionStatus}");
        }
    }
    /// <summary>
    /// Fusion.NetworkRunner�� ���� �Ǵ� ȣ��Ʈ�� ���������� ����Ǹ� �ٽ� ȣ���մϴ�.
    /// </summary>
    public void OnConnectedToServer(NetworkRunner runner)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner�� ���� �Ǵ� ȣ��Ʈ�� �������� ���� ��� �ٽ� ȣ���մϴ�.
    /// </summary>
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner�� ���ݿ��� ���� ��û�� �����ϸ� �ݹ�
    /// </summary>
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }
    /// <summary>
    /// ���� ������ ������ ��ȯ�� �� �ݹ��� ȣ��˴ϴ�
    /// </summary>
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner�� ���� �Ǵ� ȣ��Ʈ���� ������ �������� �ٽ� ȣ���մϴ�.
    /// </summary>
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }
    /// <summary>
    /// ȣ��Ʈ ���̱׷��̼� ���μ����� ���۵Ǿ��� �� �ݹ��� ȣ��˴ϴ�
    /// </summary>
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }
    /// <summary>
    /// ����� �Է��� �����ϴ� Fusion.NetworkRunner�� �ݹ�.Fusion.NetworkInput
    /// ���� ����� ������ �����ϴ�:
    /// input.set(���ο� CustomINetworkInput() { /* �� */ });
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }
    /// <summary>
    /// �Է��� ���� �� Fusion.NetworkRunner���� �ݹ��մϴ�.
    /// </summary>
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner���� ���ο� Fusion.NetworkObject�� �ԷµǾ��� �� �ݹ�
    /// </summary>
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }
    /// <summary>
    /// Fusion.NetworkRunner���� �� Fusion.NetworkObject�� ����Ǹ� �ݹ�
    /// </summary>
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }
    /// <summary>
    /// ���ο� �÷��̾ �����ϸ� Fusion.NetworkRunner���� �ݹ��մϴ�.
    /// </summary>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }
    /// <summary>
    /// �÷��̾��� ������ �������� Fusion.NetworkRunner���� �ݹ��մϴ�.
    /// </summary>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }
    /// <summary>
    /// �ŷ��� �� �ִ� ������ ��Ʈ���� �����ϰ� ������ �� �ݹ��� ȣ��˴ϴ�
    /// </summary>
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }
    /// <summary>
    /// �ŷ��� �� �ִ� ������ ��Ʈ���� ���ŵǸ� �ݹ��� ȣ��˴ϴ�
    /// </summary>
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }
    /// <summary>
    /// Scene Load�� �Ϸ�Ǹ� Callback�� ȣ��˴ϴ�
    /// </summary>
    /// <param name="runner"></param>
    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }
    /// <summary>
    /// Scene Load�� ���۵Ǹ� Callback�� ȣ��˴ϴ�
    /// </summary>
    /// <param name="runner"></param>
    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
    /// <summary>
    /// Photon���� �� ���� ����� ���� �� ȣ��˴ϴ�
    /// </summary>
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }
    /// <summary>
    /// ���ʰ� ����� �� ȣ��˴ϴ�
    /// </summary>
    /// <param name="shutdownReason">Fusion�� ����� ������ �����մϴ�</param>
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }
    /// <summary>
    /// �� �ݹ��� �������� �߼۵� �ùķ��̼� �޽����� ���ŵ� �� ȣ��˴ϴ�
    /// </summary>
    /// <param name="runner"></param>
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }




}
