using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    PlayerManager playerManager;
    PlayerLocomotion playerLocomotion;

    int horizontalHash;
    int verticalHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();

        horizontalHash = Animator.StringToHash("Horizontal");
        verticalHash = Animator.StringToHash("Vertical");

        animator.applyRootMotion = false;
    }

    /// <summary>
    /// Проигрывает заданную анимацию
    /// </summary>
    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.SetBool("isUsingRootMotion", useRootMotion);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    /// <summary>
    /// Обновление значений blend tree
    /// </summary>
    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal;
        float snappedVertical;

        // --- Snapped Horizontal ---
        if (horizontalMovement > 0 && horizontalMovement < 0.55f) snappedHorizontal = 0.5f;
        else if (horizontalMovement >= 0.55f) snappedHorizontal = 1f;
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f) snappedHorizontal = -0.5f;
        else if (horizontalMovement <= -0.55f) snappedHorizontal = -1f;
        else snappedHorizontal = 0f;

        // --- Snapped Vertical ---
        if (verticalMovement > 0 && verticalMovement < 0.55f) snappedVertical = 0.5f;
        else if (verticalMovement >= 0.55f) snappedVertical = 1f;
        else if (verticalMovement < 0 && verticalMovement > -0.55f) snappedVertical = -0.5f;
        else if (verticalMovement <= -0.55f) snappedVertical = -1f;
        else snappedVertical = 0f;

        // --- Спринт ---
        if (isSprinting)
        {
            snappedHorizontal = horizontalMovement; // оставляем полное значение для плавного поворота
            snappedVertical = 2f; // обозначаем спринт в blend tree
        }

        // Обновляем значения Animator с плавным переходом
        animator.SetFloat(horizontalHash, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(verticalHash, snappedVertical, 0.1f, Time.deltaTime);
    }

    /// <summary>
    /// Применяем Root Motion к Rigidbody
    /// </summary>
}