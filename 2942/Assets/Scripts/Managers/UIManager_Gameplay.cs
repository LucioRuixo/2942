using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Gameplay : MonoBehaviour
{
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI bombStateText;
    public TextMeshProUGUI scoreText;
    public GameObject pauseScreen;
    public GameObject endOfLevelScreen;
    public GameObject endScreen;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI finalScoreText;

    public static event Action onResumeButtonPressed;
    public static event Action onNextLevelButtonPressed;

    void OnEnable()
    {
        GameManager.onPauseStateChange += SetPauseScreenActive;
        GameManager.onGameEnd += ActivateEndScreen;

        LevelManager.onLevelEndReached += EnableEndOfLevelScreen;

        PlayerModel.onEnergyUpdate += UpdateEnergy;
        PlayerModel.onScoreUpdate += UpdateScore;
        PlayerModel.onDeath += ActivateEndScreen;
        PlayerController.onBombStateUpdate += UpdateBombState;
    }

    void OnDisable()
    {
        GameManager.onPauseStateChange -= SetPauseScreenActive;
        GameManager.onGameEnd -= ActivateEndScreen;

        LevelManager.onLevelEndReached -= EnableEndOfLevelScreen;

        PlayerModel.onEnergyUpdate -= UpdateEnergy;
        PlayerModel.onScoreUpdate -= UpdateScore;
        PlayerModel.onDeath -= ActivateEndScreen;
        PlayerController.onBombStateUpdate -= UpdateBombState;
    }

    void UpdateEnergy(int energy)
    {
        energyText.text = "ENERGY: " + energy + "%";
    }

    void UpdateScore(int score)
    {
        scoreText.text = "SCORE: " + score;
    }

    void UpdateBombState(bool bombState)
    {
        bombStateText.text = bombState ? "BOMB READY!" : "RECHARGING BOMB...";
    }

    void SetPauseScreenActive(bool state)
    {
        pauseScreen.SetActive(state);
    }

    void EnableEndOfLevelScreen()
    {
        endOfLevelScreen.SetActive(true);
    }

    void ActivateEndScreen(bool playerWon, int finalScore)
    {
        endText.text = playerWon ? "YOU WON!" : "YOU LOST!";
        finalScoreText.text = "FINAL SCORE: " + finalScore;

        endScreen.SetActive(true);
    }

    public void UnpauseGame()
    {
        if (onResumeButtonPressed != null)
            onResumeButtonPressed();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToNextLevel()
    {
        endOfLevelScreen.SetActive(false);

        if (onNextLevelButtonPressed != null)
            onNextLevelButtonPressed();
    }
}