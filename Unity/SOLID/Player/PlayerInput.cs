using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class PlayerInput : NetworkBehaviour, INetworkRunnerCallbacks
{
    private bool _callbacksAdded = false;
    public override void Spawned()
    {
        if (Runner != null && Object.HasInputAuthority)
        {
            if (!_callbacksAdded)
            {
                Runner.AddCallbacks(this);
                _callbacksAdded = true;
            }
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) {
        var inputData = new NetworkInputData
        {
            HorizontalInput = Input.GetAxis("Horizontal"),
            JumpInput = Input.GetButtonDown("Jump"),
            DashInput = Input.GetKeyDown(KeyCode.C),
            Attack1Input = Input.GetButtonDown("Fire1"),
            Attack2Input = Input.GetButtonDown("Fire2"),
            DefenseInput = Input.GetKey(KeyCode.B),
            DefenseUpInput = Input.GetKeyUp(KeyCode.B),
            SpecialAttackInput = Input.GetKeyDown(KeyCode.E)
        };
        input.Set(inputData);
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        RemoveCallbacks(runner);
    }
    private void OnDestroy() {
        if (Runner != null) {
           RemoveCallbacks(Runner);
        } else if (NetworkRunner.Instances.Count > 0) {
            RemoveCallbacks(NetworkRunner.Instances[0]);
        }
    }

    private void RemoveCallbacks(NetworkRunner runner) {
         if (runner != null && _callbacksAdded)
        {
            runner.RemoveCallbacks(this);
            _callbacksAdded = false;
        }
    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Joined " + player.AsIndex);
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Left " + player.AsIndex);
    }

    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        //implementacion a sobreescribir
    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        //implementacion a sobreescribir
    }
}