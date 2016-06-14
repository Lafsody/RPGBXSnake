using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip wallHitSound;
    public AudioClip joinPartySound;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }
    
    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }

    public void PlayWallHitSound()
    {
        audioSource.PlayOneShot(wallHitSound);
    }

    public void PlayJoinPartySound()
    {
        audioSource.PlayOneShot(joinPartySound);
    }
}
