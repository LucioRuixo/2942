using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    public enum Sounds
    {
        Button,
        PlayerShot,
        EnemyShot,
		Item,
        Explosion
    }

    public enum Songs
    {
        MainMenu,
        Gameplay
    }

    public AudioSource audioSource;

    public AudioClip[] sfxs;
    public AudioClip[] music;

    [Header("Sound Options")]
    public bool soundOn = true;
    public bool musicOn = true;

    void OnEnable()
    {
        SceneManager.sceneLoaded += PlayMusicOnNewScene;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= PlayMusicOnNewScene;
    }

    void PlayMusicOnNewScene(Scene scene, LoadSceneMode mode)
    {
        Songs song;

        switch (scene.name)
        {
            case "Main Menu":
                song = Songs.MainMenu;
                break;
            case "Gameplay":
                song = Songs.Gameplay;
                break;
            default:
                song = 0;
                break;
        }

        PlayMusic(song);
    }

    public void PlaySound(Sounds sound)
	{
		if (soundOn)
			AudioSource.PlayClipAtPoint(sfxs[(int)sound], Vector3.zero);
	}

	public void PlayMusic(Songs song)
	{
        audioSource.clip = this.music[(int)song];

		if (musicOn)
			audioSource.Play();
	}

	public void ToggleSound()
	{
		soundOn = !soundOn;
	}

	public void ToggleMusic()
	{
		musicOn = !musicOn;

		if (musicOn)
			audioSource.Play();
		else
			audioSource.Stop();
	}
}
