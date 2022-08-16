using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetlevelMaster (float sliderValue)
    {
        audioMixer.SetFloat("Master", sliderValue);
    }

    public void SetlevelMusic (float sliderValue)
    {
        audioMixer.SetFloat("Music", sliderValue);
    }

    public void SetlevelSFX (float sliderValue)
    {
        audioMixer.SetFloat("SFX", sliderValue);
    }
}
