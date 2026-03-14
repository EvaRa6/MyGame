using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int value = 1;

    public float magnetRadius = 3f;
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float jumpDuration = 0.3f;

    public AudioClip pickupSound;
    private AudioSource audioSource;

    private Transform player;
    private bool isFlying = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;


        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void FixedUpdate()
    {
        if (isFlying) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < magnetRadius)
        {
            StartCoroutine(JumpAndFlyToPlayer());
        }
    }

    IEnumerator JumpAndFlyToPlayer()
    {
        isFlying = true;

        BottleManager.instance.AddBottle(value);
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }

        Vector3 startPos = transform.position;

        float time = 0;
        while (time < jumpDuration)
        {
            time += Time.deltaTime;
            float t = time / jumpDuration;
            Vector3 pos = startPos;
            pos.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = pos;
            yield return null;
        }

        Vector3 endPos;
        while (true)
        {
            endPos = player.position;
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, endPos) < 0.3f)
                break;

            yield return null;
        }

        Destroy(gameObject);
    }
}
