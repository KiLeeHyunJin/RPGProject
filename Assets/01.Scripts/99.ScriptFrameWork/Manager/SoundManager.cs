using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource infoSource;

    public float BGMVolme { get { return bgmSource.volume; } set { bgmSource.volume = value; } }
    public float SFXVolme { get { return sfxSource.volume; } set { sfxSource.volume = value; } }
    public float InfoVolume { get { return infoSource.volume; } set { infoSource.volume = value; } }

    public void PlayBGM(AudioClip clip)
    {
        if(Equals(bgmSource.clip ,clip) == false)
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }
        }
        else
        {
            bgmSource.clip = clip;
        }
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying == false)
            return;

        bgmSource.Stop();
    }

    public void PlayButton(AudioClip clip)
    {
        if (infoSource.isPlaying)
            infoSource.Stop();
        infoSource.clip = clip;
        infoSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void StopSFX()
    {
        if (sfxSource.isPlaying == false)
            return;

        sfxSource.Stop();
    }
}
