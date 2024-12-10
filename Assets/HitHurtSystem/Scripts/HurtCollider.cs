using System;
using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    public UnityEvent <HitCollider, HurtCollider> onHurt;
    internal void NotifyHit(HitCollider hitCollider)
    {
        onHurt.Invoke(hitCollider, this);
    }
}
