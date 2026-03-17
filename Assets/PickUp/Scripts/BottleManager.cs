using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BottleManager : MonoBehaviour
{
    public static BottleManager instance;
    public TextMeshProUGUI bottleText;

    public GameObject winPanel; 
    public int totalBottles = 13;

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
            bottleText.text = bottles + " " + totalBottles;
        else
            Debug.LogWarning("Bottle Text is not assigned!");

        if (bottles >= totalBottles)
        {
            WinGame();
        }
    }

    void WinGame()
{
    Debug.Log("Победа!");

    if (winPanel != null)
        winPanel.SetActive(true);

    Time.timeScale = 0f;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    Invoke("LoadMenu", 3f);
}

void LoadMenu()
{
    Time.timeScale = 1f; 
    SceneManager.LoadScene("MainMenu");
}
}