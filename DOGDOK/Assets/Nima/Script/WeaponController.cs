using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseMaker))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] int fireRate;
    [SerializeField] int damage;
    [SerializeField] float shootRange;
    [SerializeField] float noiseRange;

    [SerializeField] bool isShooting;
    [SerializeField] bool isAutomatic;
    [SerializeField] bool isPiercing;

    [SerializeField] Transform shootPoint;
    [SerializeField] float fireInterval;
    [SerializeField] float fireTimer;
    NoiseMaker noiseMaker;
    

    private void Start()
    {
        fireInterval = (1 / (float)fireRate);
        noiseMaker = GetComponent<NoiseMaker>();
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
        }
        fireInterval = (1 / (float)fireRate);
        fireTimer += Time.deltaTime;

        if (isShooting && fireTimer >= fireInterval)
        {
            Shoot();
            
        }

    }

    void Shoot()
    {
        fireTimer = 0;
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        Debug.DrawRay(ray.origin, ray.direction * shootRange, Color.red, 0.1f);

        noiseMaker.MakeNoise(noiseRange,shootPoint);

        if (!isPiercing)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
            {
                if (hit.transform.tag != null)
                {
                    Debug.Log(hit.transform.tag);
                }
            }
        }
        else // Piercing ammo
        {
            // Define the layer mask to exclude the trigger layer (replace "Trigger" with the name of your trigger layer).
            int layerMask = ~LayerMask.GetMask("Trigger");

            // Cast the ray and get all hits along its path.
            RaycastHit[] hits = Physics.RaycastAll(ray, shootRange, layerMask);

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
