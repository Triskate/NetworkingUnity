using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static HumanoidPlayerController.PlayerController;

namespace HumanoidPlayerController
{
    public class PlayerController : NetworkBehaviour
    {
        [Header("Plane Movement")]
        [SerializeField] float walkSpeed = 5f;
        [SerializeField] float runSpeed = 10f;
        [SerializeField] float acceleration = 20f;
        [SerializeField] float decceleration = 50f;
        Vector3 currentSpeed;

        [Header("Vertical Movement")]
        [SerializeField] float jumpSpeed = 5f;
        [SerializeField] float gravity = 9.8f;

        [Header("Orientation")]
        [SerializeField] float angularSpeed = 360f;
        public enum OrientationMode
        {
            MovementDirection,
            CameraForward,
            LookAtTarget
        }
        [SerializeField] OrientationMode orientationMode = OrientationMode.MovementDirection;
        [SerializeField] Transform target;

        [Header("Input Actions")]
        [SerializeField] InputActionReference move;
        [SerializeField] InputActionReference run;
        [SerializeField] InputActionReference jump;

        CharacterController characterController;
        Camera mainCamera;
        Animator animator;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            move.action.Enable();
            run.action.Enable();
            jump.action.Enable();

            move.action.performed += OnMove;
            move.action.canceled += OnMove;

            run.action.started += OnRun;
            run.action.canceled += OnRun;

            jump.action.performed += OnJump;
        }

        Vector3 movementOnPlaneServer;
        Vector3 desiredSpeed;
        Vector3 desiredForwardServer;
        private void Update()
        {
            if (IsLocalPlayer)
            {
                // Movimiento
                Vector3 movement = mainCamera.transform.TransformDirection(rawMovement);
                float originalMagnitude = movement.magnitude;
                Vector3 movementOnPlaneClient = Vector3.ProjectOnPlane(movement, Vector3.up);
                movementOnPlaneClient = movementOnPlaneClient.normalized * originalMagnitude;

                DoMove_ServerRPC(movementOnPlaneClient);

                // Orientación
                Vector3 desiredForwardClient = Vector3.zero;
                switch (orientationMode)
                {
                    case OrientationMode.MovementDirection:
                        desiredForwardClient = movementOnPlaneClient;
                        break;
                    case OrientationMode.CameraForward:
                        desiredForwardClient = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
                        break;
                    case OrientationMode.LookAtTarget:
                        desiredForwardClient = Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up);
                        break;
                }
                DoSetDesiredForward_ServerRPC(desiredForwardClient);
            }


            if (IsServer)
            {
                // Orientación
                float angularDistance = Vector3.SignedAngle(transform.forward, desiredForwardServer, Vector3.up);
                float angularStep = angularSpeed * Time.deltaTime;
                float angleToApply = Mathf.Sign(angularDistance) * Mathf.Min(angularStep, Mathf.Abs(angularDistance));
                Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, Vector3.up);
                transform.rotation = rotationToApply * transform.rotation;

                // Animación
                Vector3 localMovementOnPlane = transform.InverseTransformDirection(movementOnPlaneServer);
                animator.SetFloat("ForwardVelocity", localMovementOnPlane.z);
                animator.SetFloat("HorizontalVelocity", localMovementOnPlane.x);
            }

        }



        // Función que envia el movimiento al servidor
        [Rpc(SendTo.Server)]
        private void DoMove_ServerRPC(Vector3 movementOnPlane)
        {
            characterController.Move(movementOnPlane * walkSpeed * Time.deltaTime);
            movementOnPlaneServer = movementOnPlane; // Actualiza el movementOnPlain para que sean iguales en cliente y servidor
            desiredSpeed = movementOnPlane * walkSpeed;
        }

        // Función que actualiza el desiredForward al servidor
        [Rpc(SendTo.Server)]
        private void DoSetDesiredForward_ServerRPC(Vector3 desiredForward)
        {
            desiredForwardServer = desiredForward;
        }

        private void OnDisable()
        {
            move.action.Disable();
            run.action.Disable();
            jump.action.Disable();

            move.action.performed -= OnMove;
            move.action.canceled -= OnMove;

            run.action.started -= OnRun;
            run.action.canceled -= OnRun;

            jump.action.performed -= OnJump;
        }

        // Funciones de movimiento
        Vector3 rawMovement;
        private void OnMove(InputAction.CallbackContext context)
        {
            if (IsLocalPlayer)
            {
                Vector2 movementValue = context.ReadValue<Vector2>();
                rawMovement = Vector3.zero;
                rawMovement.x = movementValue.x;
                rawMovement.z = movementValue.y;
            }
        }
        private void OnRun(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
        private void OnJump(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}

