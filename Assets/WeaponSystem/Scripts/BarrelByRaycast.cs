using UnityEngine;

public class BarrelByRaycast : MonoBehaviour, IHitter
{
    [SerializeField] float damage = 1f;
    [SerializeField] float range = 30f;
    [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers;

    public void Shot()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, layerMask))
        {
            hit.collider.GetComponent<HurtCollider>()?.NotifyHit(this);
        }
    }

    Transform IHitter.GetAggressor()
    {
        return transform;
    }

    float IHitter.GetDamage()
    {
        return damage;
    }

    Vector3 IHitter.GetHitOrigin()
    {
        return transform.position;
    }
}
