using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    public AnimatorManager animatorManager;
    public InputManager inputManager;

    Vector3 moveDirecton;
    Transform cameraObject;
    public Rigidbody playerRigidbody;

    [Header("Camera Transform")]
    public Transform cameraHolderTransform;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 4f;
    public float sprintingSpeed = 7f;
    public float rotationSpeed = 7f;

    [Header("Jump/Gravity")]
    public float jumpHeight = 3f;
    public float gravityIntensity = -15f;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float falllingVelocity;
    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundedLayer;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;

        // Направление движения относительно камеры
        moveDirecton = cameraObject.forward * inputManager.verticalInput;
        moveDirecton += cameraObject.right * inputManager.horizontalInput;
        moveDirecton.Normalize();
        moveDirecton.y = 0;

        // Выбираем скорость
        float speed = walkingSpeed;
        if (isSprinting) speed = sprintingSpeed;
        else if (inputManager.moveAmount > 0.5f) speed = runningSpeed;

        moveDirecton *= speed;

        // Применяем к Rigidbody, сохраняя вертикальную скорость
        Vector3 vel = moveDirecton;
        vel.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = vel;
    }

    private void HandleRotation()
    {
        if (isJumping)
            return;

        Vector3 targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y += rayCastHeightOffSet;
        targetPosition = transform.position;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            animatorManager.animator.SetBool("isUsingRootMotion", false);
            inAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * falllingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundedLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
                animatorManager.PlayTargetAnimation("Land", true);

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
        }
        else
            isGrounded = false;

        if (isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            else
                transform.position = targetPosition;
        }
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            isJumping = true;
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 vel = moveDirecton;
            vel.y = jumpingVelocity;
            playerRigidbody.velocity = vel;
        }
    }

    public void HandleDodge()
    {
        if (playerManager.isInteracting)
            return;

        animatorManager.PlayTargetAnimation("Dodge", true, true);
    }
}