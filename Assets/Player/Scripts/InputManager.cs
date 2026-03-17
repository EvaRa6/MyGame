using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    public PlayerLocomotion playerLocomotion;
    public AnimatorManager animatorManager;
    public PlayerManager player;

    public Vector2 movementInput;
    public Vector2 cameraInput;
    public FixedJoystick joystick;

    public float cameraInputX;
    public float cameraInputY;

    public float touchSensitivity = 0.1f;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool b_Input;
    public bool x_Input;
    public bool jump_Input;
    public bool interactionInput;
    public bool isSprintingToggle; // тумблер спринта

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;

            playerControls.PlayerActions.X.performed += i => x_Input = true;

            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

            playerControls.PlayerActions.Interact.performed += i => interactionInput = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleDodgeInput();
        HandleInteractionInput();
        HandleTouchCamera();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y + joystick.Vertical;
        horizontalInput = movementInput.x + joystick.Horizontal;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        // moveAmount = сила движения (для анимаций)
        moveAmount = Mathf.Clamp01(Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput));

        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        // Тумблер спринта: включен и есть движение
        playerLocomotion.isSprinting = isSprintingToggle && moveAmount > 0;
    }

    public void OnSprintButtonPressed()
    {
        isSprintingToggle = !isSprintingToggle; // переключаем состояние спринта
    }

    public void OnJumpButtonPressed()
    {
        jump_Input = true; // активируем прыжок
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleDodgeInput()
    {
        if (x_Input)
        {
            x_Input = false;
            playerLocomotion.HandleDodge();
        }
    }

    private void HandleInteractionInput()
    {
        if (interactionInput && !player.canInteract)
        {
            interactionInput = false;
        }
    }

    private void HandleTouchCamera()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                cameraInputX = touch.deltaPosition.x * touchSensitivity;
                cameraInputY = touch.deltaPosition.y * touchSensitivity;
            }
        }

#if UNITY_EDITOR
        cameraInputX = Input.GetAxis("Mouse X") * 2f;
        cameraInputY = Input.GetAxis("Mouse Y") * 2f;
#endif
    }
}