using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip mainMenuMusic;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void StartMainMenuMusic()
    {
        audioSource.clip = mainMenuMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            StartMainMenuMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
