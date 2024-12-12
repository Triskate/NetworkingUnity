using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Explosion : NetworkBehaviour, IHitter
{
    [Header("Parameters")]
    [SerializeField] float explosionRadius = 10f;
    [SerializeField] float explosionForce = 300f;

    [SerializeField] float damage = 5f;

    [Header("VFX")]
    [SerializeField] ParticleSystem explosion;

    void Start()
    {
        if (IsHost || IsServer)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            PushNearbyRigidBodies(colliders);
            DamageHurtColliders(colliders);
            Destroy(gameObject);
            InstantiateExplosionEffect_ClientRPC();
        }
    }
    private void DamageHurtColliders(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            collider.GetComponent<HurtCollider>()?.NotifyHit(this);
        }
    }
    private void PushNearbyRigidBodies(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.attachedRigidbody)
            { collider.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius); }
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void InstantiateExplosionEffect_ClientRPC()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    Vector3 IHitter.GetHitOrigin()
    {
        return transform.position;
    }

    float IHitter.GetDamage()
    {
        return damage;
    }

    Transform IHitter.GetAggressor()
    {
        return transform;
    }
}
