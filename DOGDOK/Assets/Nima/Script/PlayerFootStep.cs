using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStep : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    AudioSource footStepAudioSource;
    [Header("Foot Step")]
    [SerializeField] AudioClip footStepSound;
    [SerializeField] float footStepTimer;
    [SerializeField] float minFootStepPitch;
    [SerializeField] float maxFootStepPitch;

    [SerializeField] float maxSpeed;

    
    void Start()
    {
        footStepAudioSource = GetComponent<AudioSource>();
        playerController = GetComponentInParent<PlayerController>();
        
    }
    public void PlayFootStep()
    {
        footStepAudioSource.pitch = Random.Range(minFootStepPitch, maxFootStepPitch);
        maxSpeed = playerController.characterMovement.runningSpeed;
        footStepAudioSource.volume = playerController.characterMovement.currentVelocity / maxSpeed;
        footStepAudioSource.PlayOneShot(footStepSound);
    }

}
