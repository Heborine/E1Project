using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;

    public void SetVolume(float slidervalue){
        myAudioMixer.SetFloat("MasterVolume",Mathf.Log10(slidervalue)*20);
    }
}