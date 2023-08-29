using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (NoiseMaker))]
[RequireComponent(typeof (AudioSource))]
[RequireComponent(typeof (WeaponController))]
public class Shooting : MonoBehaviour
{
    [Header("References")]
    NoiseMaker noiseMaker;
    WeaponController weaponController;
    [Space]

    GameObject player;
    PlayerController playerController;
    AimPlayer aimPlayer;
    [SerializeField] AmmoPooling ammoPooling;
    [SerializeField] GameObject enemyHitParticle;
    [SerializeField] LayerMask bulletHitLayer;
    [SerializeField] Transform shootingPoint;

    [Header("Info")]

    [SerializeField] bool isShooting;
    [SerializeField] float fireInterval;
    [SerializeField] float fireTimer;
    [Header("WeaoponSettings")]

    [SerializeField] int fireRate = 2;
    [SerializeField] int damage = 5;
    [SerializeField] float shootRange = 100;
    [SerializeField] float noiseRange = 15.6f;
    [SerializeField] bool isAutomatic;
    [SerializeField] bool isPiercing = false;
    [SerializeField] Transform shootPoint;

    [Header("Acceleration")]
    [SerializeField] bool isAccelerating;
    [SerializeField] float accelerationDuration = 5;
    [SerializeField] float maxAcceleration = 10;
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
    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> gunSounds;
    [SerializeField] float minPitch = 0.8f;
    [SerializeField] float maxPitch = 1.2f;

    // Start is called before the first frame update
    private void Start()
    {
        noiseMaker = GetComponent<NoiseMaker>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        aimPlayer = player.GetComponent<AimPlayer>();
        audioSource = GetComponent<AudioSource>();
        weaponController = GetComponent<WeaponController>();
        fireInterval = (1 / (float)fireRate);
        fireTimer = fireInterval;
    }

    void Recoil()
    {
        isRecoilingUp = true;
        targetRecoil = Mathf.Clamp(currentRecoil + recoilMagnitude, 0, maxRecoil);
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && playerController.currentState == PlayerController.PlayerStates.Combat)
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
            accelerationTimer = 0;
        }



        if (isShooting && isAccelerating)
        {
            accelerationTimer += Time.deltaTime;
            accelerationTimer = Mathf.Clamp(accelerationTimer, 0, accelerationDuration);

            acceleration = Mathf.Lerp(1, maxAcceleration, accelerationTimer / accelerationDuration);
        }
        else
        {
            acceleration = 1;
        }
        fireInterval = (1 / (float)fireRate);
        fireTimer += Time.deltaTime * acceleration;

        if (isShooting && fireTimer >= fireInterval && weaponController.canShoot)
        {
            fireTimer = 0;
            Recoil();

            Shoot(shootPoint, playerController.aimPlayer.GetAimHitInfo(), shootRange, noiseRange, isPiercing, damage);
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, accelerationTimer / accelerationDuration);
            audioSource.PlayOneShot(gunSounds[0]);

        }

        if (isRecoilingUp)
        {
            currentRecoil = Mathf.Lerp(currentRecoil, targetRecoil, recoilSpeed * Time.deltaTime);
            aimPlayer.rotationX -= currentRecoil;
            aimPlayer.rotationY += Random.Range(-currentRecoil, currentRecoil);
            if (Mathf.Abs(currentRecoil - targetRecoil) < recoilTreshold)
            {
                isRecoilingUp = false;
                isRecoilingDown = true;
            }
        }
        else if (isRecoilingDown)
        {
            currentRecoil = Mathf.Lerp(currentRecoil, 0, recoilSpeed * Time.deltaTime);
            aimPlayer.rotationX += currentRecoil;

            if (currentRecoil < recoilTreshold)
            {
                currentRecoil = 0;
                isRecoilingDown = false;
            }
        }
    }

    public void Shoot(Transform _shootingPoint,Vector3 _targetPosition, float _shootingRange,float _noiseRange,bool _isPiercing, int _damage)
    {

        Vector3 _direction = (_targetPosition - _shootingPoint.position).normalized;
        Ray ray = new Ray(_shootingPoint.position, _direction);
        shootingPoint.forward = ray.direction;
        Debug.DrawRay(ray.origin, ray.direction * _shootingRange, Color.red, 0.1f);

        noiseMaker.MakeNoise(_noiseRange, _shootingPoint);
        
        if (!_isPiercing)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, _shootingRange,bulletHitLayer))
            {
                if (hit.collider.tag != null)
                {
                    ammoPooling.SpawnAmmo(hit.distance);
                    Debug.Log(hit.transform.tag);
                    if (hit.collider.CompareTag("EnemyBodyPart"))
                    {
                        Instantiate(enemyHitParticle,hit.point,hit.transform.rotation);
                        hit.collider.gameObject.GetComponent<EnemyBodyPartDamageDetection>().GetPartDamage(_damage);
                    }
                }
                
            }
            else
            {
                ammoPooling.SpawnAmmo(50);
            }

        }
        else // Piercing ammo
        {
            // Define the layer mask to exclude the trigger layer (replace "Trigger" with the name of your trigger layer).

            // Cast the ray and get all hits along its path.
            RaycastHit[] hits = Physics.RaycastAll(ray, _shootingRange, bulletHitLayer);

            // Process each hit along the ray's path.
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                // Handle the hit here (e.g., apply damage to enemies, etc.).
                Debug.Log("Piercing Hit: " + hit.transform.tag);

                // If you want to stop the ray from passing through objects after hitting the first one,
                // you can use a break statement here to exit the loop.
                // break;
            }
        }
    }
}
