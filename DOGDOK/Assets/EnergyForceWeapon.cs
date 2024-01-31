using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseMaker))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(WeaponController))]
public class EnergyForceWeapon : MonoBehaviour
{
    [Header("References")]
    NoiseMaker noiseMaker;
    WeaponController weaponController;
    GameObject player;
    PlayerController playerController;
    AimPlayer aimPlayer;
    AudioSource audioSource;

    [Header("WeaponInfo")]
    [SerializeField] float energyUsage = 10f;
    Quaternion defaultRotation;
    [SerializeField] int damage;
    [SerializeField] List<Vector3> pushRotations;
    [SerializeField] bool isImpulsive;
    [SerializeField] List<Rigidbody> enemyRbList;
    [SerializeField] bool isPushing;
    [SerializeField] float pushDuration = 0.3f;
    [SerializeField] float pushTimer;
    [SerializeField] float pushForce;
    [SerializeField] int forceAngleRange = 60;
    [SerializeField] float forceRange = 10f;
    [SerializeField] Transform pivotPosition;
    [Header("FireTimer")]
    [SerializeField] float FiringInterval = 2f;
    [SerializeField] float firingTimer;
    [Header("Extra Setting")]
    [SerializeField] float pushSmoothing;
    [SerializeField] GameObject forceEffect;
    [SerializeField] float forceEffectMaxScale = 10;
    [SerializeField] Material forceEffectMaterial;
    [SerializeField] float effectSpeedMult = 10f;
    
    private void Awake()
    {
        noiseMaker = GetComponent<NoiseMaker>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        aimPlayer = player.GetComponent<AimPlayer>();
        audioSource = GetComponent<AudioSource>();
        weaponController = GetComponent<WeaponController>();
        forceEffectMaterial = forceEffect.GetComponent<Renderer>().material;
    }

    private void Start()
    {
        defaultRotation = forceEffect.transform.localRotation;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && weaponController.canShoot && firingTimer >= FiringInterval && playerController.playerEnergyController.UseEnergy(energyUsage))
        {
            Fire();
        }

        //firingTimer += Time.deltaTime;//niþan almýyorken saymýyor

        
    }
    private void FixedUpdate()
    {
        if (isPushing)
        {

            PushEnemies();
            pushTimer -= Time.fixedDeltaTime;
            float _effectRatio = Mathf.Clamp01(((pushDuration - pushTimer) * effectSpeedMult) / pushDuration) ;

            forceEffect.transform.localScale = Vector3.one * _effectRatio * forceEffectMaxScale ;
            forceEffectMaterial.SetFloat("_AlphaTress", _effectRatio);
            forceEffectMaterial.SetFloat("_EffectOfset", _effectRatio);
            if (_effectRatio >= 1)
            {
                //return the effect to it's parent
                forceEffect.transform.parent = pivotPosition.transform;
                forceEffect.transform.localRotation= defaultRotation;
                forceEffect.transform.localPosition = Vector3.zero;

                forceEffect.SetActive(false);
            }
            if (pushTimer <= 0)
            {
                isPushing = false;
            }
        }
        firingTimer += Time.fixedDeltaTime;
    }
    void OnDrawGizmosSelected()
    {
        // Calculate the direction of the weapon's forward vector.
        Vector3 weaponDirection = pivotPosition.forward;

        // Draw rays to visualize the shotgun spread.
        for (int i = -5; i <= 5; i++) // Adjust the number of rays as needed.
        {
            float angle = forceAngleRange * i / 10.0f;
            Quaternion spreadRotation = Quaternion.AngleAxis(angle, pivotPosition.up);
            Vector3 direction = spreadRotation * weaponDirection;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(pivotPosition.position, direction * forceRange);
        }
    }

    void PushEnemies()
    {
        for (int i = 0; i < enemyRbList.Count; i++)
        {
            Rigidbody _enemy = enemyRbList[i];
            Vector3 _pushDirection = pushRotations[i];
            float _currentPushForece = Mathf.Lerp(pushForce / 10, pushForce,  Mathf.Pow((pushTimer/ pushDuration), pushSmoothing));

            if (isImpulsive)
            {
                _enemy.AddForce(_pushDirection * _enemy.mass * _currentPushForece, ForceMode.Impulse);
            }
            else
            {
                _enemy.AddForce(_pushDirection * _enemy.mass * _currentPushForece, ForceMode.Force);
            }
        }
        
    }

    void Fire()
    {
        enemyRbList.Clear();
        pushRotations.Clear();
        forceEffect.transform.parent = player.transform;//send the effect the out to keep the position with weapon rotation
        forceEffect.SetActive(true);
        firingTimer = 0;
        // Calculate the direction of the weapon's forward vector.
        Vector3 weaponDirection = pivotPosition.forward;

        // Cast multiple rays with a spread.
        for (int i = -5; i <= 5; i++) // Adjust the number of rays as needed.
        {
            float angle = forceAngleRange * i / 10.0f;
            Quaternion spreadRotation = Quaternion.AngleAxis(angle, pivotPosition.up);
            Vector3 direction = spreadRotation * weaponDirection;

            RaycastHit[] hits = Physics.RaycastAll(pivotPosition.position, direction, forceRange);

            // Cast a ray in the spread direction.
            
            foreach (RaycastHit _hit in hits)
            {
                if (_hit.collider.CompareTag("Enemy"))
                {
                    // Handle enemy hit, apply damage, etc.
                    GameObject _enemyObject = _hit.collider.gameObject;
                    
                    Rigidbody _enemyRb = _enemyObject.GetComponent<Rigidbody>();
                    if (!enemyRbList.Contains(_enemyRb))
                    {
                        int _damage = Mathf.RoundToInt(damage * ((forceRange - _hit.distance) / forceRange));

                        _enemyObject.GetComponent<EnemyHealth>().GetDamage(_damage, EnemyHealth.HitSource.Player,transform);

                        Vector3 pushDirection = (_hit.point - pivotPosition.position).normalized;
                        pushRotations.Add(pushDirection * (forceRange - _hit.distance) / forceRange);
                        enemyRbList.Add(_enemyRb);
                    }
                }
            }
        }
        isPushing = true;
        pushTimer = pushDuration;
    }
}
