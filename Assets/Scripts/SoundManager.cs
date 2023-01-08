using System;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip endGameSound;
    [SerializeField] private AudioClip playerHitSound;
    [SerializeField] private AudioClip zoneTakenSound;
    [SerializeField] private AudioClip colorRefilledSound;


    private void Awake()
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

    private void Start()
    {
        PlayBackgroundMusic();

        GameManager.Instance.gameOverEvent += PlayEndGameSound;
    }

    private void OnDestroy()
    {
        GameManager.Instance.gameOverEvent -= PlayEndGameSound;
    }

    public void PlayZoneTakenSound(){
        AudioSource.PlayClipAtPoint(zoneTakenSound, Vector3.zero);
    }

    public void PlayColorRefillSound()
    {
        AudioSource.PlayClipAtPoint(colorRefilledSound, Vector3.zero);
    }

    public void PlayPlayerHitSound()
    {
        AudioSource.PlayClipAtPoint(playerHitSound, Vector3.zero);
    }
    
    public void PlayBackgroundMusic()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayEndGameSound(int noInt)
    {
        audioSource.clip = endGameSound;
        audioSource.Play();
    }
}
