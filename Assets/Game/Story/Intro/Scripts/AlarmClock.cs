using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock : MonoBehaviour
{
    public Material clockDigits;
    public bool actived;

    AudioSource _audio;
    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        if (!actived)
        {
            clockDigits.SetColor("_EmissionColor", Color.black);
        }
    }
    public void Activate()
    {
        clockDigits.SetColor("_EmissionColor", Color.white);
        _audio.Play();
    }
}
