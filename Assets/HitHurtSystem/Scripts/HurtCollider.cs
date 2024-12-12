using System;
using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    public UnityEvent <IHitter, HurtCollider> onHurt;
    internal void NotifyHit(IHitter hitter)
    {
        onHurt.Invoke(hitter, this);
    }
}
