using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instace;

    public AudioSource audioSource;




    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instace != null)
            Destroy(this);
        else
            instace = this;
    }



    public void PlayAudio(AudioClip sound, bool overrideSound = false)
    {
        if (audioSource.isPlaying && !overrideSound) return;
        audioSource.clip = sound;
        audioSource.Play();
    }
}
