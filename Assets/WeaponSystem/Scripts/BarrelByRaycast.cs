using UnityEngine;

public class BarrelByRaycast : MonoBehaviour
{
    [SerializeField] float range = 30f;
    [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers;
    public void Shot()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, layerMask))
        {
            // hit.collider
        }
    }
}
