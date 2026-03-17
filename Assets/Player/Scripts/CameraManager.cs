using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public InputManager inputManager;
<<<<<<< HEAD

<<<<<<< HEAD
    public Transform cameraPivot;   // Pivot камеры (относительно головы)
=======
    
    public Transform cameraPivot;
>>>>>>> parent of 26b544a (update for mobile)
=======
    public Transform cameraPivot;   // pivot для вертикального вращения
>>>>>>> parent of 3d35a70 (fixed camera manager and player movement)
    public Camera cameraObject;
    public GameObject player;
    
    Vector3 cameraFollowVelocity = Vector3.zero;
    Vector3 targetPosition;
    Vector3 cameraRotation;
    Quaternion targetRotation;
    
    [Header("Camera Speeds")]
    public float cameraSmoothTime = 0.2f;
    
    float lookAmountVertical;
    float lookAmountHorizontal;
    float maximumPivotAngle = 15;
    float minimumPivotAngle = -15;
    

<<<<<<< HEAD
    [Header("Camera Settings")]
    public float followSpeed = 10f;       // скорость следования камеры
    public float rotateSpeed = 5f;        // скорость вращения камеры
    public Vector3 cameraOffset = new Vector3(0, 1.8f, -3f); // смещение камеры от игрока

    private float lookHorizontal;
    private float lookVertical;
    private float minVertical = -35f;
    private float maxVertical = 60f;

<<<<<<< HEAD
=======
>>>>>>> parent of 26b544a (update for mobile)
=======
    // Для совместимости с PlayerManager
>>>>>>> parent of 3d35a70 (fixed camera manager and player movement)
    public void HandleAllCameraMovement()
    {
        FollowPlayer();
        RotateCamera();
    }
    
    private void FollowPlayer()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        // камера всегда позади игрока с учетом его поворота
        Vector3 targetPosition = player.position + player.rotation * offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // камера смотрит туда же, куда и игрок
        transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, followSpeed * Time.deltaTime);
=======
        targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraFollowVelocity, cameraSmoothTime * Time.deltaTime);
        transform.position = targetPosition;
>>>>>>> parent of 26b544a (update for mobile)
=======
        // Камера всегда находится позади игрока + offset
        Vector3 desiredPosition = player.position + new Vector3(0, cameraOffset.y, 0) + player.forward * cameraOffset.z;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
>>>>>>> parent of 3d35a70 (fixed camera manager and player movement)
    }

    private void RotateCamera()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        float smoothX = inputManager.cameraInputX * cameraSensitivity;
        float smoothY = inputManager.cameraInputY * cameraSensitivity;

        lookHorizontal += smoothX;
        lookVertical -= smoothY;

        lookVertical = Mathf.Clamp(lookVertical, minVertical, maxVertical);

        // Плавное вращение игрока и pivot
        player.rotation = Quaternion.Slerp(player.rotation, Quaternion.Euler(0, lookHorizontal, 0), rotateSpeed * Time.deltaTime);
        cameraPivot.localRotation = Quaternion.Slerp(cameraPivot.localRotation, Quaternion.Euler(lookVertical, 0, 0), rotateSpeed * Time.deltaTime);
=======
        lookAmountVertical = lookAmountVertical + (inputManager.cameraInputX);
        lookAmountHorizontal = lookAmountHorizontal - (inputManager.cameraInputY);
        lookAmountHorizontal = Mathf.Clamp(lookAmountHorizontal, minimumPivotAngle, maximumPivotAngle);

        cameraRotation = Vector3.zero;
        cameraRotation.y = lookAmountVertical;
        targetRotation = Quaternion.Euler(cameraRotation);
        targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraSmoothTime);
        transform.rotation = targetRotation;
        
        cameraRotation = Vector3.zero;
        cameraRotation.x = lookAmountHorizontal;
        targetRotation = Quaternion.Euler(cameraRotation);
        targetRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, cameraSmoothTime);
        cameraPivot.localRotation = targetRotation;
>>>>>>> parent of 26b544a (update for mobile)
=======
        // Свайп пальцем
        lookHorizontal += inputManager.cameraInputX;
        lookVertical -= inputManager.cameraInputY;
        lookVertical = Mathf.Clamp(lookVertical, minVertical, maxVertical);

        // Поворот игрока по горизонтали
        Quaternion playerRotation = Quaternion.Euler(0, lookHorizontal, 0);
        player.rotation = Quaternion.Slerp(player.rotation, playerRotation, rotateSpeed * Time.deltaTime);

        // Поворот камеры по вертикали через pivot (голова)
        Quaternion pivotRotation = Quaternion.Euler(lookVertical, 0, 0);
        cameraPivot.localRotation = Quaternion.Slerp(cameraPivot.localRotation, pivotRotation, rotateSpeed * Time.deltaTime);
>>>>>>> parent of 3d35a70 (fixed camera manager and player movement)
    }
}
