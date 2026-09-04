using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using TMPro;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using UnityEngine;

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
    public float actionTime = 20f;      // Ultimele cate secunde vom rula melodia din regiunea Action


    [SerializeField, BoxGroup("FMOD Events")]
    private EventReference musicEvent;


    private bool gameIsActive = true;   // false cand se termina timpul
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
        UpdateScoreUI();
        musicEventInstance = RuntimeManager.CreateInstance(musicEvent);
        musicEventInstance.setParameterByName("MusicState", 0);
        musicEventInstance.start();
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
