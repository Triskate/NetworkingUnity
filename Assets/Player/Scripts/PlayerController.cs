using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float speed = 4f;

    [Header("Input Actions")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference bomb;

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

        if (IsServer || IsHost)
        {
            rigidbody.velocity = movementFromClient * speed;
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
    void OnJump(InputAction.CallbackContext ctx)
    {
        Vector3 jump = new Vector3(0f,0.1f,0f);
        rigidbody.AddForce(jump, ForceMode.Impulse);
    }
    void OnBomb(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnBomb");
    }

    [Rpc(SendTo.Server)]
    void SetMoveValue_ServerRpc(Vector3 moveValue)
    {
        movementFromClient = moveValue;
    }
}
