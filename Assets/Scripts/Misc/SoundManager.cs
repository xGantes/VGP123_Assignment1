using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public new AudioSource audio;
    public AudioClip roar;
    public AudioClip cool;
    public AudioClip hurt;
    public AudioClip stomp;
    public AudioClip chamAtk;

    public static SoundManager soundInstances;

    //SoundManager.soundInstances.audio.PlayOneShot(SoundManager.soundInstances.roar);

    private void Awake()
    {
        if (soundInstances != null && soundInstances != this)
        {
            Destroy(this.gameObject);
            return;
        }
        soundInstances = this;
        DontDestroyOnLoad(this);
    }
}
