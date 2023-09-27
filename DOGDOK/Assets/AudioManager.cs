using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSouce;
    public AudioClip menuClickButton;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSouce = GetComponent<AudioSource>();
    }
    public void PlayAudio(AudioClip _audio)
    {
        audioSouce.PlayOneShot(_audio);
    }
}
