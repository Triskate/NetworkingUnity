using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class HumanoidLife : NetworkBehaviour
{
    [SerializeField] float startingLife = 1f;
    [SerializeField] float currentLife;

    public UnityEvent<HumanoidLife, float> onLifeChanged;
    public UnityEvent<HumanoidLife> onLifeDepleted;

    HurtCollider hurtCollider;

    private void Awake()
    {
        hurtCollider = GetComponent<HurtCollider>();
        currentLife = startingLife;
    }

    private void OnEnable()
    {
        hurtCollider.onHurt.AddListener(OnHurt);
    }

    private void OnDisable()
    {
        hurtCollider.onHurt.RemoveListener(OnHurt);
    }

    private void OnHurt(IHitter hitter, HurtCollider hurtCollider)
    {
        if (IsLocalPlayer)
        {
            GetHurted(hitter.GetDamage());
        }
        else
        {
            GetHurted_ServerRPC(hitter.GetDamage());
        }
    }

    internal void RestoreToStartingLife()
    {
        currentLife = startingLife;
    }
    private void GetHurted(float damage)
    {
        if (currentLife > 0)
        {
            currentLife -= damage;
            onLifeChanged.Invoke(this, currentLife);
            if (currentLife < 0) { onLifeDepleted.Invoke(this); }
        }
    }

    [Rpc(SendTo.Server)]
    private void GetHurted_ServerRPC(float damage)
    {
        GetHurted_ClientRPC(damage);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void GetHurted_ClientRPC(float damage)
    {
        if (IsLocalPlayer)
        {
            GetHurted(damage);
        }
    }
}
