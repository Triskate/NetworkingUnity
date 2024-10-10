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
    [SerializeField] TextMeshPro text;
    [SerializeField] float timeToExplode = 4;
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
        }

        if (IsClient || IsHost)
        {
            Debug.Log(timeLeftToExplode.Value);
        }

        //this.GetComponent<NetworkObject>().Despawn();
    }

    private void OnValueChanged(float previousValue, float newValue)
    {
        int time = (int)newValue;
        text.SetText(time.ToString());
    }
}
