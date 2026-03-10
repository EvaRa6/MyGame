using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    
    Vector3 moveDirecton;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    public float movementSpeed = 7;
    public float rotationSpeed = 15;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }


    public void HandleMovement()
    {
        moveDirecton = cameraObject.forward * inputManager.verticalInput;
        moveDirecton = moveDirecton + cameraObject.right * inputManager.horizontalInput;
        moveDirecton.Normalize();
        moveDirecton.y = 0;
        moveDirecton = moveDirecton * movementSpeed;

        Vector3 movementVelocity = moveDirecton;
        playerRigidbody.velocity = movementVelocity;
    }

    public void Handlerotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.Lookrotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;

    }
}
