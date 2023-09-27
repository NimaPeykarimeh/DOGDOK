using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip mainMenuMusic;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.clip = mainMenuMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
