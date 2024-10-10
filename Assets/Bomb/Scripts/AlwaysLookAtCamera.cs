using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.LookAt(transform.position - mainCamera.transform.forward, Vector3.up);
    }
}
