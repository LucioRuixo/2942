using TMPro;
using UnityEngine;

public class UIManager_Gameplay : MonoBehaviour
{
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI bombStateText;

    void OnEnable()
    {
        PlayerModel.onLifeUpdate += UpdateLife;
        PlayerController.onBombStateUpdate += UpdateBombState;
    }

    void OnDisable()
    {
        PlayerModel.onLifeUpdate -= UpdateLife;
        PlayerController.onBombStateUpdate -= UpdateBombState;
    }

    void UpdateLife(int life)
    {
        lifeText.text = "LIFE: " + life.ToString();
    }

    void UpdateBombState(bool bombState)
    {
        bombStateText.text = bombState ? "BOMB READY!" : "RECHARGING BOMB...";
    }
}