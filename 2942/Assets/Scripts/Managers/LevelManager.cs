using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int currentLevel = 1;

    public List<GameObject> levels;

    public static event Action onLevelEndReached;
    public static event Action onNewLevelSetting;
    public static event Action onLastLevelCompleted;

    void OnEnable()
    {
        UIManager_Gameplay.onNextLevelButtonPressed += SetNextLevel;

        PlayerController.onLevelEndReached += CheckIfPlayerWon;
    }

    void Start()
    {
        if (onNewLevelSetting != null)
            onNewLevelSetting();
    }

    void OnDisable()
    {
        UIManager_Gameplay.onNextLevelButtonPressed -= SetNextLevel;

        PlayerController.onLevelEndReached -= CheckIfPlayerWon;
    }

    void CheckIfPlayerWon()
    {
        if (currentLevel < levels.Count)
        {
            if (onLevelEndReached != null)
                onLevelEndReached();
        }
        else
        {
            if (onLastLevelCompleted != null)
                onLastLevelCompleted();
        }
    }

    void SetNextLevel()
    {
        levels[currentLevel - 1].SetActive(false);
        levels[currentLevel].SetActive(true);
        currentLevel++;

        if (onNewLevelSetting != null)
            onNewLevelSetting();
    }
}