using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjectDeactivator : NetworkBehaviour
{
    public enum Condition
    {
        IsClient,
        IsServer,
        IsHost,
        IsOwner,
        IsNotOwner,
    }
    [SerializeField] Condition deactivationCondition;

    private void Start()
    {
        gameObject.SetActive(!ConditionMeet());
    }

    private bool ConditionMeet()
    {
        switch (deactivationCondition)
        {
            case Condition.IsClient: return IsClient;
            case Condition.IsServer: return IsServer;
            case Condition.IsHost:   return IsHost;
            case Condition.IsOwner: return IsOwner;
            case Condition.IsNotOwner: return !IsOwner;
        }
        return false;
    }
}
