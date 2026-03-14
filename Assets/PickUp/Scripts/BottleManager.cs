using TMPro;
using UnityEngine;

public class BottleManager : MonoBehaviour
{
    public static BottleManager instance;
    public TextMeshProUGUI bottleText;

    int bottles = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBottle(int amount)
    {
        bottles += amount;
        if (bottleText != null)
            bottleText.text = "Bottles: " + bottles;
        else
            Debug.LogWarning("Bottle Text is not assigned!");
    }
}
