using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private float survivalTime = 300f;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject victoryPanel;

    private float timer;
    private bool hasWon;

    private void Start()
    {
        victoryPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (hasWon) return;

        timer += Time.deltaTime;

        float remainingTime = survivalTime - timer;
        remainingTime = Mathf.Max(0f, remainingTime);

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";

        if (timer >= survivalTime)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        hasWon = true;

        Debug.Log("Victory");
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
