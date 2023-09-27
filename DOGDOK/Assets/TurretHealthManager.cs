using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealthManager : MonoBehaviour
{
    TurretController turretController;
    [SerializeField] int maxHealth = 200;
    [SerializeField] int currentHealth;
    public List<EnemyController> alertedEnemiesList;
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
        foreach (EnemyController _enemy in alertedEnemiesList)
        {
            _enemy.isTargetedTurret = false;
            _enemy.AlertEnemy(false,false,false,transform);
        }
        alertedEnemiesList.Clear();
        Destroy(gameObject);
    }
}
