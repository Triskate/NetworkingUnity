using UnityEngine;
using DG.Tweening;
using System;
using Unity.Netcode;

[RequireComponent (typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float startSpeed = 10f;
    [SerializeField] float lifeTime = 10f;
    [SerializeField] float lifeDurationAfterCollision = 0f;
    [SerializeField] GameObject explosionPrefab;

    Rigidbody rb;
    Tween lifeTimeTween;
    Tween afterCollisionTween;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.linearVelocity = transform.forward * startSpeed;
        lifeTimeTween = DOVirtual.DelayedCall(lifeTime, () => Destroy(gameObject));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(afterCollisionTween == null ) 
        {
            lifeTimeTween.Kill();
            DOVirtual.DelayedCall(lifeDurationAfterCollision, () =>
            {
                Destroy(gameObject);
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.GetComponent<NetworkObject>().Spawn();
            });
        }
    }
}
