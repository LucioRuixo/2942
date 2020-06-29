using TMPro;
using UnityEngine;

public class UIManager_Gameplay : MonoBehaviour
{
    public TextMeshProUGUI lifeText;

    void OnEnable()
    {
        PlayerModel.onLifeUpdate += UpdateLife;
    }

    void OnDisable()
    {
        PlayerModel.onLifeUpdate -= UpdateLife;
    }

    void UpdateLife(int life)
    {
        lifeText.text = "LIFE: " + life.ToString();
    }
}