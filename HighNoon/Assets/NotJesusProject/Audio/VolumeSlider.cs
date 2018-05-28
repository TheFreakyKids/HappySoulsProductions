using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer masterMixer;
    public void SetMasterLvl(float masLvl)
    {
        masterMixer.SetFloat("MasterVolume", masLvl);
    }
}
