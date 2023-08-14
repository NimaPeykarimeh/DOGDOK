using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (NoiseMaker))]

public class Shooting : MonoBehaviour
{
    
    NoiseMaker noiseMaker;
    [SerializeField] AmmoPooling ammoPooling;
    [SerializeField] GameObject enemyHitParticle;
    [SerializeField] LayerMask bulletHitLayer;
    // Start is called before the first frame update
    void Start()
    {
        noiseMaker = GetComponent<NoiseMaker>();
    }

    public void Shoot(Transform _shootingPoint,Vector3 _targetPosition, float _shootingRange,float _noiseRange,bool _isPiercing, int _damage)
    {

        Vector3 _direction = (_targetPosition - _shootingPoint.position).normalized;
        Ray ray = new Ray(_shootingPoint.position, _direction);
        Debug.DrawRay(ray.origin, ray.direction * _shootingRange, Color.red, 0.1f);

        noiseMaker.MakeNoise(_noiseRange, _shootingPoint);

        if (!_isPiercing)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, _shootingRange,bulletHitLayer))
            {
                if (hit.transform.tag != null)
                {
                        ammoPooling.SpawnAmmo(hit.distance);
                    if (hit.transform.CompareTag("EnemyBodyPart"))
                    {
                        Instantiate(enemyHitParticle,hit.point,hit.transform.rotation);
                        hit.transform.gameObject.GetComponent<EnemyBodyPartDamageDetection>().GetPartDamage(_damage);
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
