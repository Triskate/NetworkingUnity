using System;
using Unity.Netcode;
using UnityEngine;

public class HumanoidDeath : NetworkBehaviour
{
    HumanoidLife humanoidLife;
    private void Awake()
    {
        humanoidLife = GetComponent<HumanoidLife>();
    }

    private void OnEnable()
    {
        humanoidLife.onLifeDepleted.AddListener(OnLifeDepleted);
    }
    private void OnDisable()
    {
        humanoidLife.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }

    private void OnLifeDepleted(HumanoidLife humanoidLife)
    {
        if (IsServer) 
        {
            Die();
            Invoke(nameof(Resurrect), 3f);
        }
    }

    void Die()
    {
        Die_ClientRPC();
        gameObject.SetActive(false);
    }
    void Resurrect()
    {
        Resurrect_ClientRPC();
        gameObject.SetActive(true);
    }

    [Rpc(SendTo.ClientsAndHost)]
    void Die_ClientRPC()
    {
        gameObject.SetActive(false);
        humanoidLife.RestoreToStartingLife();
    }

    [Rpc(SendTo.ClientsAndHost)]
    void Resurrect_ClientRPC()
    {
        gameObject.SetActive(true);
    }
}
