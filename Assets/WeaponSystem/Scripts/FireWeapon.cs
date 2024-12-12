using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    BarrelByRaycast[] barrels;

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
        barrels = GetComponentsInChildren<BarrelByRaycast>();
    }
    public void Shot()
    {
        foreach (BarrelByRaycast b in barrels)
        {
            b.Shot();
        }
    }
}
