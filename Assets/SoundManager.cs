using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioClip zoneTakenSound;
    [SerializeField] private AudioClip colorRefilledSound;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayZoneTakenSound(){
        AudioSource.PlayClipAtPoint(zoneTakenSound, Vector3.zero);
    }

    public void PlayColorRefillSound()
    {
        AudioSource.PlayClipAtPoint(colorRefilledSound, Vector3.zero);
    }
}
