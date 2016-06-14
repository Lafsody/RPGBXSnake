using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }


    public AudioSource bgmSource;
    public AudioSource audioSource;

    // BGM
    public AudioClip normalBGM;
    public AudioClip bossFightBGM;

    // SFX
    public AudioClip hitSound;
    public AudioClip wallHitSound;
    public AudioClip joinPartySound;
    public AudioClip bossAppearSound;
    public AudioClip winBossSound;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }
    
    public void PlayNormalBGM()
    {
        bgmSource.PlayOneShot(normalBGM);
    }

    public void PlayBossFightBGM()
    {
        bgmSource.PlayOneShot(bossFightBGM);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
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

    public void PlayBossAppearSound()
    {
        audioSource.PlayOneShot(bossAppearSound);
    }

    public void PlayWinBossSound()
    {
        audioSource.PlayOneShot(winBossSound);
    }
}
