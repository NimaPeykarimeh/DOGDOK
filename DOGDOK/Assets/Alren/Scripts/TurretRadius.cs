using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRadius : MonoBehaviour
{
    [SerializeField] private TurretFireController TurretFireController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TurretFireController.EnemyList.Add(other.transform);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        TurretFireController.EnemyList.Remove(other.transform);
    //    }
    //}
}
