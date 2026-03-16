using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public InputManager inputManager;

    public Transform cameraPivot;   // pivot для вертикального вращения
    public Camera cameraObject;
    public Transform player;        // Transform игрока

    [Header("Camera Settings")]
    public float followSpeed = 10f;       // скорость следования камеры
    public float rotateSpeed = 5f;        // скорость вращения камеры
    public Vector3 cameraOffset = new Vector3(0, 1.8f, -3f); // смещение камеры от игрока

    private float lookHorizontal;
    private float lookVertical;
    private float minVertical = -35f;
    private float maxVertical = 60f;

    // Для совместимости с PlayerManager
    public void HandleAllCameraMovement()
    {
        FollowPlayer();
        RotateCamera();
    }

    private void FollowPlayer()
    {
        // Камера всегда находится позади игрока + offset
        Vector3 desiredPosition = player.position + new Vector3(0, cameraOffset.y, 0) + player.forward * cameraOffset.z;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
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
    }
}