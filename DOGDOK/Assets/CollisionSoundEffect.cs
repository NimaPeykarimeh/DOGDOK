using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionSoundEffect : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip hitAudio;

    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float hitSpeedThreshold = 1f;
    float hitSpeed;

    float hitTimer = 0.15f;
    bool canSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canSound)
        {
            canSound = false;
            hitSpeed = collision.relativeVelocity.magnitude;
            if (hitSpeed >= hitSpeedThreshold)
            {
                audioSource.pitch = Random.Range(0.85f, 1.1f);
                audioSource.volume = Mathf.Clamp01(hitSpeed / maxSpeed);
                audioSource.PlayOneShot(hitAudio);    
            }

        }
    }

    private void FixedUpdate()
    {
        if (!canSound)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0)
            {
                canSound = true;
                hitTimer = 0.15f;
            }
        }
    }
}
