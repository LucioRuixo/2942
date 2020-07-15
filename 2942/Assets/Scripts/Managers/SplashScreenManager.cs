using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public float fadeWaitTime;
    public float fadeTime;
    public float displayTime;
    float alphaStep;

    public TextMeshProUGUI aGameBy;
    public TextMeshProUGUI companyName;
    public TextMeshProUGUI title;

    void Start()
    {
        alphaStep = fadeWaitTime / fadeTime;

        StartCoroutine(ShowCompanyLogo(fadeWaitTime));
    }

    IEnumerator ShowCompanyLogo(float waitTime)
    {
        for (float t = 0f; t < fadeTime; t += waitTime)
        {
            if (aGameBy) aGameBy.alpha += alphaStep;
            if (companyName) companyName.alpha += alphaStep;

            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(displayTime);

        for (float t = 0f; t < fadeTime; t += waitTime)
        {
            if (aGameBy) aGameBy.alpha -= alphaStep;
            if (companyName) companyName.alpha -= alphaStep;

            yield return new WaitForSeconds(waitTime);
        }

        StartCoroutine(ShowTitle(fadeWaitTime));
    }

    IEnumerator ShowTitle(float waitTime)
    {
        for (float t = 0f; t < fadeTime; t += waitTime)
        {
            if (title) title.alpha += alphaStep;

            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(displayTime);

        for (float t = 0f; t < fadeTime; t += waitTime)
        {
            if (title) title.alpha -= alphaStep;

            yield return new WaitForSeconds(waitTime);
        }

        SceneManager.LoadScene("Main Menu");
    }
}