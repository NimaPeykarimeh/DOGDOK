using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStep : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    AudioSource audioSource;
    [Header("Foot Step")]
    [SerializeField] AudioClip footStepSound;
    [SerializeField] float footStepTimer;
    [SerializeField] float minFootStepPitch;
    [SerializeField] float maxFootStepPitch;

    [SerializeField] float maxSpeed;

    
    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        playerController = GetComponentInParent<PlayerController>();
        
    }
    public void PlayFootStep()
    {
        audioSource.pitch = Random.Range(minFootStepPitch, maxFootStepPitch);
        maxSpeed = playerController.characterMovement.runningSpeed;
        audioSource.volume = playerController.characterMovement.currentVelocity / maxSpeed;
        audioSource.PlayOneShot(footStepSound);
    }

}
