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
    }

    public void ViewInstructionsMenu()
    {
        if (mainMenu) mainMenu.SetActive(false);
        if (instructionsMenu) instructionsMenu.SetActive(true);
    }

    public void ViewCreditsMenu()
    {
        if (mainMenu) mainMenu.SetActive(false);
        if (creditsMenu) creditsMenu.SetActive(true);
    }

    public void Return()
    {
        if (instructionsMenu && instructionsMenu.activeSelf)
            instructionsMenu.SetActive(false);
        else if (creditsMenu)
            creditsMenu.SetActive(false);

        if (mainMenu) mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}