using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int value = 1;

    public float magnetRadius = 3f;
    public float moveSpeed = 5f;
    public float acceleration = 15f;

    Transform player;
    Rigidbody rb;

    float currentSpeed;

    void Start()
    {
        rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < magnetRadius)
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;

            Vector3 newPosition = Vector3.MoveTowards(
                rb.position,
                player.position,
                currentSpeed * Time.fixedDeltaTime
            );

            rb.MovePosition(newPosition);

            if (distance < 0.5f)
            {
                Collect();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected");
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched bottle");

            if (BottleManager.instance != null)
            {
                BottleManager.instance.AddBottle(value);
            }
            else
            {
                Debug.LogError("BottleManager not found in scene!");
            }

            Destroy(gameObject);
        }
    }

    void Collect()
    {
        //BottleManager.instance.AddBottle(value);
        Destroy(gameObject);
    }
}
