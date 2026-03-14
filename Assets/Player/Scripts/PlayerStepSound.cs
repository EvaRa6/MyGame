using System.Collections;
using UnityEngine;

public class PlayerStepSound: MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;        // AudioSource на игроке
    public AudioClip[] walkClips;          // звуки ходьбы
    public AudioClip[] runClips;           // звуки бега

    [Header("Step Settings")]
    public float walkStepRate = 0.5f;      // время между шагами
    public float runStepRate = 0.35f;      // время между шагами

    private Rigidbody rb;
    private float stepTimer = 0f;

    void Start()
    {
        // если не привязан, создаём AudioSource
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D звук

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb == null) return;

        // движение по горизонтали
        Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        bool isMoving = horizontalVel.magnitude > 0.1f;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayStepSound(isRunning);
                stepTimer = isRunning ? runStepRate : walkStepRate;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayStepSound(bool running)
    {
        AudioClip[] clips = running ? runClips : walkClips;
        if (clips.Length == 0) return;

        int index = Random.Range(0, clips.Length);
        audioSource.pitch = Random.Range(0.9f, 1.1f); // случайный pitch
        audioSource.PlayOneShot(clips[index]);
    }
}
