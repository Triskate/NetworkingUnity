using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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

    private void Update()
    {
        //rigidbody.velocity = rawMove * speed;

        Vector3 forward = rawMove.z * mainCamera.transform.forward;
        Vector3 right = rawMove.x * mainCamera.transform.right;
        Vector3 movement = Vector3.ProjectOnPlane(forward + right, Vector3.up).normalized;

        rigidbody.velocity = movement * speed;
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
        Debug.Log("OnJump");
    }
    void OnBomb(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnBomb");
    }
}
