using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    
    Vector3 moveDirecton;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    public bool isSprinting;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 7;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }


    private void HandleMovement()
    {
        moveDirecton = cameraObject.forward * inputManager.verticalInput;
        moveDirecton = moveDirecton + cameraObject.right * inputManager.horizontalInput;
        moveDirecton.Normalize();
        moveDirecton.y = 0;


        if (isSprinting)
        {
            moveDirecton = moveDirecton * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
            moveDirecton = moveDirecton * runningSpeed;
            }
            else
            {
            moveDirecton = moveDirecton * walkingSpeed;
            }
        }


        moveDirecton = moveDirecton * runningSpeed;

        Vector3 movementVelocity = moveDirecton;
        playerRigidbody.linearVelocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;

    }
}
