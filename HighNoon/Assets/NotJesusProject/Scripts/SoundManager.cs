using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public GameObject audioItemSFX;
    public GameObject audioItemVFX;
    public GameObject audioItemMX;
    public GameObject audioItemH;
    private GameObject go;
    private GameObject prefabBus;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip, string bus)
    {
        if (bus == "sfx")
        {
            prefabBus = audioItemSFX;
        }
        else if (bus == "vfx")
        {
            prefabBus = audioItemVFX;
        }
        else if (bus == "mx")
        {
            prefabBus = audioItemMX;
        }
        else if (bus == "swap")
        {
            prefabBus = audioItemH;
        }

        go = (GameObject)Instantiate(prefabBus);
        AudioSource src = go.GetComponent<AudioSource>();
        src.clip = clip;
        RandomPitch();
        src.Play();
        Destroy(go, clip.length);
    }
    
    public void RandomPitch()
    {
        if (prefabBus = audioItemH)
        {
            audioItemH.GetComponent<AudioSource>().pitch = (Random.Range(0.95f, 1.05f));
        }
    }

    public void StopClip()
    {
        Destroy(go);
    }
}