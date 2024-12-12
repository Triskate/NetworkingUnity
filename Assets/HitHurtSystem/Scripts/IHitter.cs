using UnityEngine;

public interface IHitter
{
    public Vector3 GetHitOrigin();
    public float GetDamage();
    public Transform GetAggressor();

}
