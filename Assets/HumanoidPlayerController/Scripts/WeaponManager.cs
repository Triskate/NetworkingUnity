using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField] InputActionReference changeWeapon;
    [SerializeField] InputActionReference shoot;
    FireWeapon[] fireWeapons;
    int currentWeapon = -1;

    private void Awake()
    {
        fireWeapons = GetComponentsInChildren<FireWeapon>();
    }

    private void OnEnable()
    {
        changeWeapon.action.Enable();
        changeWeapon.action.performed += OnChangeWeapon;

        shoot.action.Enable();
        shoot.action.performed += OnShoot;
    }


    private void OnDisable()
    {
        changeWeapon.action.Disable();
        changeWeapon.action.performed -= OnChangeWeapon;

        shoot.action.Disable();
        shoot.action.performed -= OnShoot;
    }


    private void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (IsLocalPlayer)
        {
            Vector2 delta = context.ReadValue<Vector2>();

            if (currentWeapon != -1) { fireWeapons[currentWeapon].NotifyDeselected(); }

            if (delta.y > 0)
            {
                currentWeapon++;
                if (currentWeapon >= fireWeapons.Length) { currentWeapon = -1; }
            }
            else
            {
                currentWeapon--;
                if (currentWeapon < -1) { currentWeapon = fireWeapons.Length - 1; }
            }

            if (currentWeapon != -1) { fireWeapons[currentWeapon].NotifySelected(); }
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if(currentWeapon != -1)
        {
            fireWeapons[currentWeapon].Shot();
        }
    }
}
