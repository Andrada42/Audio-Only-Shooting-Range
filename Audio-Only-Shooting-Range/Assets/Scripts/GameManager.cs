using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    [Header("Score")]
    public int score = 0;

    [Header("Time")]
    public float timeRemaining = 60f;


    private bool gameIsActive = true;   // false cand se termina timpul


    void Awake()    // Singleton
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    void Update()
    {
        if (!gameIsActive)
            return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            gameIsActive = false;
            Debug.Log("Timpul a expirat!");
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = "Time: " + seconds.ToString() + "s";
        }
    }

    public void AddToScore(int amount)
    {
        if (!gameIsActive)
            return;

        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
