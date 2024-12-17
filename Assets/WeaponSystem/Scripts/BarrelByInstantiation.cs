using UnityEngine;

public class BarrelByInstantiation : Barrel
{

    [SerializeField] GameObject projectilePrefab;

    public override void Shot()
    {
        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }
}
