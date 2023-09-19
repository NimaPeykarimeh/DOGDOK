using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealthManager : MonoBehaviour
{
    TurretController turretController;
    [SerializeField] int maxHealth = 200;
    [SerializeField] int currentHealth;
    private void Awake()
    {
        turretController = GetComponent<TurretController>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void GetDamage(int _damage)
    {
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
            DestroyTurret();
        }
    }

    void DestroyTurret()
    {
        Destroy(gameObject);
    }
}
