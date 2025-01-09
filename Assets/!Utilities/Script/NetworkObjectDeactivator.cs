using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjectDeactivator : NetworkBehaviour
{
    public enum Condition
    {
        IsClient,
        IsServer,
        IsHost
    }
    [SerializeField] Condition deactivationCondition;

    private void Start()
    {
        enabled = !ConditionMeet();
    }

    private bool ConditionMeet()
    {
        switch (deactivationCondition)
        {
            case Condition.IsClient: return IsClient;
            case Condition.IsServer: return IsServer;
            case Condition.IsHost:   return IsHost;
        }
        return false;
    }
}
