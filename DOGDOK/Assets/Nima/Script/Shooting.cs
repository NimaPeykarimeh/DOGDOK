using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (NoiseMaker))]

public class Shooting : MonoBehaviour
{

    NoiseMaker noiseMaker;
    [SerializeField] AmmoPooling ammoPooling;
    
    // Start is called before the first frame update
    void Start()
    {
        noiseMaker = GetComponent<NoiseMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Transform _shootingPoint, float _shootingRange,float _noiseRange,bool _isPiercing)
    {

        
        Ray ray = new Ray(_shootingPoint.position, _shootingPoint.forward);
        Debug.DrawRay(ray.origin, ray.direction * _shootingRange, Color.red, 0.1f);

        noiseMaker.MakeNoise(_noiseRange, _shootingPoint);

        if (!_isPiercing)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, _shootingRange))
            {
                if (hit.transform.tag != null)
                {
                        ammoPooling.SpawnAmmo(hit.distance);
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
            int layerMask = ~LayerMask.GetMask("Trigger");

            // Cast the ray and get all hits along its path.
            RaycastHit[] hits = Physics.RaycastAll(ray, _shootingRange, layerMask);

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
