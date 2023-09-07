using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyAreaDisable : MonoBehaviour
{
    private List<GameObject> turrets = new();

    private void Start()
    {
        //Destroy(gameObject, 10);
    }
    private void OnDestroy()
    {
        for(int i = 0; i < turrets.Count; i++)
        {
            if (turrets[i] == null)
            {
                turrets.RemoveAt(i);
                i = 0;
            }
                
        }
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].GetComponent<TurretFireController>().canShoot = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            other.gameObject.GetComponent<TurretFireController>().canShoot = true;
            turrets.Add(other.gameObject);
        }
    }
}