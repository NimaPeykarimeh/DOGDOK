using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Shooting))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] Shooting shooting;


    [SerializeField] bool isShooting;

    [SerializeField] float fireInterval;
    [SerializeField] float fireTimer;

    [Header("WeaoponSettings")]
    [SerializeField] int fireRate;
    [SerializeField] int damage;
    [SerializeField] float shootRange;
    [SerializeField] float noiseRange;
    [SerializeField] bool isAutomatic;
    [SerializeField] bool isPiercing;
    [SerializeField] Transform shootPoint;
    [Header("Acceleration")]
    [SerializeField] bool isAccelerating;
    [SerializeField] float accelerationDuration = 5;
    [SerializeField] float maxAcceleration = 2;
    [SerializeField] float accelerationTimer;
    [SerializeField] float acceleration = 1;

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> gunSounds;
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;

    private void Start()
    {
        shooting = GetComponent<Shooting>();
        fireInterval = (1 / (float)fireRate);
        fireTimer = fireInterval;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isShooting = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isShooting = false;

            accelerationTimer = 0;
        }

        if (isShooting && isAccelerating)
        {
            accelerationTimer += Time.deltaTime;
            accelerationTimer = Mathf.Clamp(accelerationTimer,0,accelerationDuration);

            acceleration = Mathf.Lerp(1,maxAcceleration,accelerationTimer/accelerationDuration);
        }
        else
        {
            acceleration = 1;
        }
        fireInterval = (1 / (float)fireRate);
        fireTimer += Time.deltaTime * acceleration;

        if (isShooting && fireTimer >= fireInterval)
        {
            fireTimer = 0;
            shooting.Shoot(shootPoint,shootRange,noiseRange,isPiercing);
            audioSource.pitch = Mathf.Lerp(minPitch,maxPitch,accelerationTimer/accelerationDuration);
            audioSource.PlayOneShot(gunSounds[0]);

        }

    }

    
}
