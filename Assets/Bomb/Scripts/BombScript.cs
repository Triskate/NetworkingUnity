using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.ParticleSystem;

public class BombScript : NetworkBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject explosionPrefab;

    [Header("Parameters")]
    [SerializeField] float timeToExplode = 4;

    [Header("Countdown")]
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
        if (IsHost || IsServer) 
        {
           GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
           explosion.GetComponent<NetworkObject>()?.Spawn();
        }
    }
}
