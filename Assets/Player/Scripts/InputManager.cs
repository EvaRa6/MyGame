using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("References")]
    public PlayerManager player;
    public PlayerLocomotion playerLocomotion;
    public AnimatorManager animatorManager;
    public CameraManager cameraManager;

    [Header("Controls")]
    public FixedJoystick joystick;

    private PlayerControls playerControls;

    [Header("Movement")]
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Camera")]
    public float cameraInputX;
    public float cameraInputY;
    public float touchSensitivity = 0.1f;

    [Header("Actions")]
    public bool b_Input;      // Sprint toggle
    public bool x_Input;      // Dodge
    public bool jump_Input;
    public bool interactionInput;

    private bool sprintToggle; // Для переключения спринта

    private void Awake()
    {
        if (!animatorManager) animatorManager = GetComponent<AnimatorManager>();
        if (!playerLocomotion) playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            // Movement
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            // Actions
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
        HandleJumpInput();
        HandleDodgeInput();
        HandleInteractionInput();
        HandleCameraInput();
    }

    private void HandleMovementInput()
    {
        // Джой + InputSystem
        verticalInput = movementInput.y + joystick.Vertical;
        horizontalInput = movementInput.x + joystick.Horizontal;

        // Расчёт moveAmount для анимации
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        // Обновляем анимации (правильно: horizontal и vertical)
        animatorManager.UpdateAnimatorValues(horizontalInput, verticalInput, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
{
    if (b_Input)
    {
        b_Input = false; // сбрасываем, чтобы не спамилось
        playerLocomotion.isSprinting = !playerLocomotion.isSprinting;
    }
}

    private void HandleJumpInput()
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
        if (interactionInput)
        {
            if (!player.canInteract)
            {
                interactionInput = false;
            }
        }
    }

    private void HandleCameraInput()
{
    // Джой + InputSystem
    cameraInputX = cameraInput.x + joystick.Horizontal;
    cameraInputY = cameraInput.y + joystick.Vertical;

    // Сенсорный ввод
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Moved)
        {
            cameraInputX += touch.deltaPosition.x * touchSensitivity;
            cameraInputY += touch.deltaPosition.y * touchSensitivity;
        }
    }

    // Мышь (для редактора)
#if UNITY_EDITOR
    cameraInputX += Input.GetAxis("Mouse X") * 2f;
    cameraInputY += Input.GetAxis("Mouse Y") * 2f;
#endif

    // Если используешь CameraManager, убери эту строку, она больше не нужна
    // cameraManager.cameraInput = new Vector2(cameraInputX, cameraInputY);
}
}