using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public float existingVolume = 0;
    public Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        audioMixer.GetFloat("MasterVolume", out existingVolume);
        volumeSlider.value = existingVolume;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
}
