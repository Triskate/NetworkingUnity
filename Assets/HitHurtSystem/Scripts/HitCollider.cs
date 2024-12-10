using UnityEngine;
using UnityEngine.Events;

public class HitCollider : MonoBehaviour
{
    public UnityEvent<HitCollider, HurtCollider> onHit;
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
        if(hurtCollider != null)
        {
            hurtCollider.NotifyHit(this);
            onHit.Invoke(this, hurtCollider);
        }
    }
}
