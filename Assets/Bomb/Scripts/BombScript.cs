using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BombScript : NetworkBehaviour
{
    [Header("Parameters")]
    [SerializeField] float timeToExplode = 4;
    [SerializeField] float explosionRadius = 10.0F;
    [SerializeField] float explosionForce = 300.0F;

    [SerializeField] TextMeshPro countdownText;

    NetworkVariable<float> timeLeftToExplode = new();


    private void Start()
    {
        timeLeftToExplode.Value = timeToExplode;
        timeLeftToExplode.OnValueChanged += OnValueChanged;
    }

    void Update()
    {
        if (IsHost || IsServer)
        {
            timeLeftToExplode.Value -= Time.deltaTime;
            if(timeLeftToExplode.Value <= 0) Explode();
        }
    }

    private void OnValueChanged(float previousValue, float newValue)
    {
        if (IsClient || IsHost)
        {
            countdownText.text = $"{Mathf.CeilToInt(newValue)}";
        }
    }

    void Explode()
    {
        if(IsHost || IsServer) 
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionForce);
            foreach (Collider collider in colliders)
            {
                if (collider.attachedRigidbody)
                {
                    collider.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
            Destroy(gameObject);
        }
    }
}
