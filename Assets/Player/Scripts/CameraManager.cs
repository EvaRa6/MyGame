using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public InputManager inputManager;

    public Transform cameraPivot;   // Pivot камеры (относительно головы)
    public Camera cameraObject;
    public Transform player;        // Transform игрока

    [Header("Camera Settings")]
    public float followSpeed = 10f;    // скорость следования за игроком
    public float rotateSpeed = 5f;     // скорость вращения камеры
    public Vector3 offset = new Vector3(0, 1.8f, -3f); // смещение камеры от головы

    private float lookHorizontal;
    private float lookVertical;
    private float minVertical = -35f;
    private float maxVertical = 60f;
    public float cameraSensitivity = 0.05f;

    public void HandleAllCameraMovement()
    {
        FollowPlayer();
        RotateCamera();
    }

    private void FollowPlayer()
    {
        // камера всегда позади игрока с учетом его поворота
        Vector3 targetPosition = player.position + player.rotation * offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // камера смотрит туда же, куда и игрок
        transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, followSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
        float smoothX = inputManager.cameraInputX * cameraSensitivity;
        float smoothY = inputManager.cameraInputY * cameraSensitivity;

        lookHorizontal += smoothX;
        lookVertical -= smoothY;

        lookVertical = Mathf.Clamp(lookVertical, minVertical, maxVertical);

        // Плавное вращение игрока и pivot
        player.rotation = Quaternion.Slerp(player.rotation, Quaternion.Euler(0, lookHorizontal, 0), rotateSpeed * Time.deltaTime);
        cameraPivot.localRotation = Quaternion.Slerp(cameraPivot.localRotation, Quaternion.Euler(lookVertical, 0, 0), rotateSpeed * Time.deltaTime);
    }
}