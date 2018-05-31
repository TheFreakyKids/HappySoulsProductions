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
        
    }

    public void LoadLevelOne()
    {
        SoundManager.instance.StopClip();
        SceneManager.LoadScene("map 3");
    }

    public void LoadLevelTwo()
    {
        SoundManager.instance.StopClip();
        SceneManager.LoadScene("High Noon Map (2)");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}