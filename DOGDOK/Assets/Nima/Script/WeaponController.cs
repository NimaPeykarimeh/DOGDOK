using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Shooting))]
public class WeaponController : MonoBehaviour
{

    [Header("Reference")]
    [SerializeField] PlayerController playerController;
    [SerializeField] Shooting shooting;
    [SerializeField] AimPlayer aimPlayer;
    [Space]

    [SerializeField] bool isShooting;
    [SerializeField] float fireInterval;
    [SerializeField] float fireTimer;

    [Header("WeaoponSettings")]
    [SerializeField] Weapon weapon;
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
    [Header("Recoil")]
    [SerializeField] float recoilTreshold = 0.1f;
    [SerializeField] bool isRecoilingUp;
    [SerializeField] bool isRecoilingDown;

    [SerializeField] float maxRecoil = 0.3f;
    [SerializeField] float recoilMagnitude = 1f;
    [SerializeField] float recoilSpeed = 10f;
    float currentRecoil;
    float targetRecoil;
    [Header("Info")]
    [SerializeField] WeaponType weaponType;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> gunSounds;
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;

    public enum WeaponType
    {
        OneHanded,
        TwoHanded,
        Melee
    }

    private void Start()
    {
        shooting = GetComponent<Shooting>();
        fireInterval = (1 / (float)fireRate);
        fireTimer = fireInterval;
        weaponType = weapon.weaponType;
    }

    void Recoil()
    {
        isRecoilingUp = true;
        targetRecoil = Mathf.Clamp(currentRecoil +  recoilMagnitude,0,maxRecoil);
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

        if (isShooting && fireTimer >= fireInterval && playerController.currentState == PlayerController.PlayerStates.Combat)
        {
            fireTimer = 0;
            Recoil();
            
            shooting.Shoot(shootPoint,playerController.aimPlayer.GetAimHitInfo(),shootRange,noiseRange,isPiercing,damage);
            audioSource.pitch = Mathf.Lerp(minPitch,maxPitch,accelerationTimer/accelerationDuration);
            audioSource.PlayOneShot(gunSounds[0]);

        }

        if (isRecoilingUp)
        {
            currentRecoil = Mathf.Lerp(currentRecoil, targetRecoil,recoilSpeed * Time.deltaTime);
            aimPlayer.rotationX -= currentRecoil;
            aimPlayer.rotationY += Random.Range(-currentRecoil, currentRecoil);
            if ( Mathf.Abs(currentRecoil - targetRecoil) < recoilTreshold)
            {
                isRecoilingUp = false;
                isRecoilingDown = true;
            }
        }
        else if(isRecoilingDown)
        {
            currentRecoil = Mathf.Lerp(currentRecoil,0, recoilSpeed * Time.deltaTime);
            aimPlayer.rotationX += currentRecoil;
            
            if (currentRecoil < recoilTreshold)
            {
                currentRecoil = 0;
                isRecoilingDown = false;
            }
        }
    }

    
}
