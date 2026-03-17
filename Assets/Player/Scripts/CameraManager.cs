using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;       // персонаж
    public Transform cameraPivot;  // пустышка для наклона
    public Camera mainCamera;      // сама камера

    [Header("Rotation")]
    public float rotationSpeed = 150f; // скорость вращения камеры
    public float minPivot = -35f;
    public float maxPivot = 35f;

    private float lookAngle;   // горизонтальное вращение
    private float pivotAngle;  // вертикальное вращение

    [HideInInspector]
    public Vector2 cameraInput; // ввод с джойстика/мыши

    private Vector3 offset; // фиксированное смещение камеры относительно персонажа

    private void Start()
    {
        // сохраняем изначальное смещение камеры
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        FollowPlayer();
        RotateCameraSmooth();
    }

    private void FollowPlayer()
    {
        // камера повторяет позицию персонажа с сохранённым смещением
        transform.position = player.position + offset;
    }

    private void RotateCameraSmooth()
    {
        // горизонтальный поворот камеры
        lookAngle += cameraInput.x * rotationSpeed * Time.deltaTime;

        // вертикальный наклон камеры
        pivotAngle -= cameraInput.y * rotationSpeed * Time.deltaTime;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        // целевой поворот вокруг персонажа
        Quaternion targetRotation = Quaternion.Euler(0, lookAngle + player.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        // плавный наклон камеры по вертикали
        Quaternion pivotTargetRotation = Quaternion.Euler(pivotAngle, 0, 0);
        cameraPivot.localRotation = Quaternion.Slerp(cameraPivot.localRotation, pivotTargetRotation, 10f * Time.deltaTime);
    }
}