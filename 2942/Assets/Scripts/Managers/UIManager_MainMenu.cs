using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instructionsMenu;
    public GameObject creditsMenu;

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");

        SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    public void ViewInstructionsMenu()
    {
        if (mainMenu) mainMenu.SetActive(false);
        if (instructionsMenu) instructionsMenu.SetActive(true);

        SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    public void ViewCreditsMenu()
    {
        if (mainMenu) mainMenu.SetActive(false);
        if (creditsMenu) creditsMenu.SetActive(true);

        SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    public void Return()
    {
        if (instructionsMenu && instructionsMenu.activeSelf)
            instructionsMenu.SetActive(false);
        else if (creditsMenu)
            creditsMenu.SetActive(false);

        if (mainMenu) mainMenu.SetActive(true);

        SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    public void Quit()
    {
        Application.Quit();

        SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }
}