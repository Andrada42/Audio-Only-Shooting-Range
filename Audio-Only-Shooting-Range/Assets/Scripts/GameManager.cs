using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using TMPro;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    [Header("UI Paneld")]
    public GameObject gameplayPanel;
    public GameObject gameOverPanel;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalScoreText;

    [Header("Score")]
    public int score = 0;

    [Header("Time")]
    public float timeRemaining = 60f;
    public float actionTime = 30f;      // Ultimele cate secunde vom rula melodia din regiunea Action


    [SerializeField, BoxGroup("FMOD Events")]
    private EventReference musicEvent;

    [SerializeField, ReadOnly]          // Read-Only pt Editor
    private bool gameIsActive = true;   // false cand se termina timpul

    public bool GameIsActive => gameIsActive;   // Read-Only din alte scripturi

    private float gameRoundDuration;
    private int musicState = 0;
    private EventInstance musicEventInstance;

    void Awake()    // Singleton
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gameRoundDuration = timeRemaining;

        musicEventInstance = RuntimeManager.CreateInstance(musicEvent);
        musicEventInstance.setParameterByName("MusicState", 0);
        musicEventInstance.start();

        StartGameRound();
    }

    public void StartGameRound()
    {
        gameIsActive = true;
        score = 0;
        timeRemaining = gameRoundDuration;

        // Ascunde cursorul si il blocheaza in mijlocul ecranului
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (gameplayPanel != null)
            gameplayPanel.SetActive(true);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void OnDestroy()
    {
        musicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);    // pentru sunetele looped
        musicEventInstance.release();
    }

    void Update()
    {
        if (!gameIsActive)
            return;

        if (timeRemaining <= actionTime && musicState == 0)
        {
            musicState = 1;
            musicEventInstance.setParameterByName("MusicState", musicState);
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
            EndGameRound();
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

    void EndGameRound()
    {
        Debug.Log("<color=orange>Timpul a expirat!</color>");

        gameIsActive = false;
        timeRemaining = 0;

        musicState = 0;
        musicEventInstance.setParameterByName("MusicState", 0);

        // Stergem tintele ramase
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach(GameObject target in targets)   // OBS: Noi vom avea mereu doar un element
            Destroy(target);

        if (gameplayPanel != null)
            gameplayPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + score;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
