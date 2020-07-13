using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool onPause = false;

    [HideInInspector] public float leftScreenLimit;
    [HideInInspector] public float rightScreenLimit;
    [HideInInspector] public float lowerScreenLimit;
    [HideInInspector] public float upperScreenLimit;

    [HideInInspector] public Vector2 screenBounds;

    public static event Action<float, float, float, float> onScreenLimitsSetting;
    public static event Action<bool> onPauseStateChange;
    public static event Action<bool, int> onGameEnd;

    void OnEnable()
    {
        UIManager_Gameplay.onResumeButtonPressed += ChangePauseState;

        PlayerModel.onDeath += ProccessGameEnd;
        PlayerModel.onVictory += ProccessGameEnd;
    }

    void Start()
    {
        Vector3 position = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        screenBounds = Camera.main.ScreenToWorldPoint(position);

        SetScreenLimits();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            ChangePauseState();
        }
    }

    void OnDisable()
    {
        UIManager_Gameplay.onResumeButtonPressed -= ChangePauseState;

        PlayerModel.onDeath -= ProccessGameEnd;
        PlayerModel.onVictory -= ProccessGameEnd;
    }

    void SetScreenLimits()
    {
        leftScreenLimit = screenBounds.x * -1;
        rightScreenLimit = screenBounds.x;
        lowerScreenLimit = screenBounds.y;
        upperScreenLimit = screenBounds.y * -1;

        if (onScreenLimitsSetting != null)
            onScreenLimitsSetting(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
    }

    void ChangePauseState()
    {
        Time.timeScale = onPause ? 1f : 0f;
        onPause = !onPause;

        if (onPauseStateChange != null)
            onPauseStateChange(onPause);
    }

    void ProccessGameEnd(bool playerWon, int finalScore)
    {
        if (onGameEnd != null)
            onGameEnd(playerWon, finalScore);
    }
}