using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip menuMusic;
    public void Awake()
    {
        SoundManager.instance.Play(menuMusic, "mx");
    }
    public void PlayGame()
    {
        SoundManager.instance.StopClip();
        SceneManager.LoadScene("AltDemo"/*SceneManager.GetActiveScene().buildIndex + 1*/);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}