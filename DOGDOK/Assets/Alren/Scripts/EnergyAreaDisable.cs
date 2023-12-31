using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyAreaDisable : MonoBehaviour
{
    private List<GameObject> turrets = new();

    private void Start()
    {
        AreaManager.areasMesh.Add(gameObject.GetComponent<MeshRenderer>());
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnDestroy()
    {
        AreaManager.areasMesh.Remove(gameObject.GetComponent<MeshRenderer>());
        for (int i = 0; i < turrets.Count; i++)
        {
            if (turrets[i] == null)
            {
                turrets.RemoveAt(i);
                i = 0;
            }
                
        }
        for (int i = 0; i < turrets.Count; i++)
        {
            if (turrets[i] != null)
                turrets[i].GetComponent<TurretController>().itHasEnergy = false;
        }
    }

    private void OnTriggerEnter(Collider other)//turret controller i�ine int connectedEnergy ekle
    {
        if (other.CompareTag("Turret"))
        {
            if (other.gameObject.TryGetComponent<TurretController>(out TurretController _controller))
            {
                _controller.itHasEnergy = true;//change it to turretController
                turrets.Add(other.gameObject);
            }
             
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            if (other.gameObject.TryGetComponent<TurretController>(out TurretController _controller))
            {
                _controller.itHasEnergy = false;
            }
            //other.gameObject.GetComponent<TurretController>().itHasEnergy = false;
        }
    }
    private void OnTriggerStay(Collider other)//turret controller i�ine int connectedEnergy ekle
    {
        if (other.CompareTag("Turret"))
        {
            if (other.gameObject.TryGetComponent<TurretController>(out TurretController _controller))
            {
                _controller.itHasEnergy = true;
            }
            //turrets.Add(other.gameObject);
        }
    }
}