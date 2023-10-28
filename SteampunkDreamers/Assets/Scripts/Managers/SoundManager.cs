using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    private static AudioSource obstacleAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        obstacleAudioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(bool loop, AudioClip clip)
    {
        obstacleAudioSource.loop = loop;
        if (!loop)
        {
            obstacleAudioSource.PlayOneShot(clip);
        }
        else
        {
            obstacleAudioSource.clip = clip;
            obstacleAudioSource.Play();
        }
    }

    public void StopAudio()
    {
        obstacleAudioSource.Stop();
    }
}
