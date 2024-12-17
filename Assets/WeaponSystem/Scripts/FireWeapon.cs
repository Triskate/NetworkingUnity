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
    public void Shot()
    {
        foreach (Barrel b in barrels)
        {
            b.Shot();
        }
    }
}
