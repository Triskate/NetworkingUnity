using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float speed = 4f;
    [SerializeField] float jumpVelocity = 5f;
    bool grounded;

    [Header("Bomb")]
    [SerializeField] GameObject bombObject;

    [Header("Input Actions")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference bomb;

    // Components
    new Rigidbody rigidbody;
    Camera mainCamera;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        move.action.Enable();
        jump.action.Enable();
        bomb.action.Enable();

        move.action.performed += OnMove;
        move.action.canceled += OnMove;
        jump.action.performed += OnJump;
        bomb.action.performed += OnBomb;
    }

    Vector3 movementFromClient = Vector3.zero;
    private void Update()
    {

        //rigidbody.velocity = rawMove * speed;

        if (IsLocalPlayer) 
        {
            Vector3 forward = rawMove.z * mainCamera.transform.forward;
            Vector3 right = rawMove.x * mainCamera.transform.right;
            Vector3 movement = Vector3.ProjectOnPlane(forward + right, Vector3.up).normalized;

            SetMoveValue_ServerRpc(movement);
            //rigidbody.velocity = movement * speed;
        }
    }

    private void FixedUpdate()
    {
        if (IsServer || IsHost)
        {
            Vector3 finalMovement = movementFromClient * speed;
            finalMovement.y = rigidbody.velocity.y;

            if (performJump)
            {
                performJump = false;
                {
                    finalMovement.y = jumpVelocity;
                }
            }

            rigidbody.velocity = finalMovement;
        }
    }

    private void OnDisable()
    {
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;
        jump.action.performed -= OnJump;
        bomb.action.performed -= OnBomb;

        move.action.Disable();
        jump.action.Disable();
        bomb.action.Disable();
    }

    Vector3 rawMove;
    void OnMove(InputAction.CallbackContext ctx) 
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        rawMove = Vector3.zero;
        rawMove.x = value.x;
        rawMove.z = value.y;
    }
    bool performJump = false;
    void OnJump(InputAction.CallbackContext ctx)
    {
        if (IsLocalPlayer && grounded) 
        {
            PerformJump_ServerRpc(); 
        }
    }
    void OnBomb(InputAction.CallbackContext ctx)
    {
        if (IsLocalPlayer)
        {
            LayDownBomb_ServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    void SetMoveValue_ServerRpc(Vector3 moveValue)
    {
        movementFromClient = moveValue;
    }
    [Rpc(SendTo.Server)]

    void LayDownBomb_ServerRpc()
    {
        GameObject newBomb = Instantiate(bombObject, transform.position, transform.rotation);
        newBomb.GetComponent<NetworkObject>().Spawn();
    }

    [Rpc(SendTo.Server)]
    void PerformJump_ServerRpc()
    {
        performJump = true;
    }

    // Check if grounded
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            grounded = true;
        }  
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            grounded = false;
        }
    }
}
