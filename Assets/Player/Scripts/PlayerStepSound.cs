using System.Collections;
using UnityEngine;

public class PlayerStepSound: MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Walk Sounds")]
    public AudioClip[] walkClips;

    [Header("Run Sounds")]
    public AudioClip[] runClips;

    [Header("Step Settings")]
    public float walkStepRate = 0.5f; 
    public float runStepRate = 0.35f;

    private float stepTimer = 0f;
    private bool isMoving = false;
    private bool isRunning = false;

    private CharacterController controller;

    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller == null) return;

        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        isMoving = horizontalVelocity.magnitude > 0.1f;

        isRunning = Input.GetKey(KeyCode.LeftShift); 

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayStepSound();
                stepTimer = isRunning ? runStepRate : walkStepRate;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayStepSound()
    {
        AudioClip[] clips = isRunning ? runClips : walkClips;
        if (clips.Length == 0) return;

        int index = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[index]);
    }
}
