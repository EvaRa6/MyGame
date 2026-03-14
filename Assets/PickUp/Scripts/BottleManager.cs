using TMPro;
using UnityEngine;

public class BottleManager : MonoBehaviour
{
    public static BottleManager instance;

    public TextMeshProUGUI bottleText;

    int bottles = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddBottle(int amount)
    {
        bottles += amount;
        bottleText.text = "Bottles: " + bottles;
    }
}