using System;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    Barrel[] barrels;

    [Header("Debug")]
    [SerializeField] bool debugShot;

    void OnValidate()
    {
        if (debugShot)
        {
            debugShot = false;
            Shot();
        }
    }

    private void Awake()
    {
        barrels = GetComponentsInChildren<Barrel>();
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Shot()
    {
        foreach (Barrel b in barrels)
        {
            b.Shot();
        }
    }

    internal void NotifySelected()
    {
        gameObject.SetActive(true);
    }

    internal void NotifyDeselected()
    {
        gameObject.SetActive(false);
    }
}
