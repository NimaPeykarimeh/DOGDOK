using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSoundManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip airAmbienteSound;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = airAmbienteSound;
        audioSource.Play();
    }

}
