using UnityEngine;
using UnityEngine.Events;

public class HitCollider : MonoBehaviour, IHitter
{
    public UnityEvent<HitCollider, HurtCollider> onHit;
    [SerializeField] float damage;
    private void OnTriggerEnter(Collider other)
    {
        CheckCollider(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollider(collision.collider);
    }

    private void CheckCollider(Collider collider)
    {
        HurtCollider hurtCollider = collider.GetComponent<HurtCollider>();
        if(hurtCollider)
        {
            hurtCollider.NotifyHit(this);
            onHit.Invoke(this, hurtCollider);
        }
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