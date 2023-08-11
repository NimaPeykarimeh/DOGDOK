using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class EnemyHealth : MonoBehaviour
{
    EnemyController enemyController;
    [SerializeField] int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Dead()
    {
        enemyController.enemySpawner.BackToPooler(transform);
        currentHealth = maxHealth;// change later
    }

    public void GetDamage(int _damage)
    {
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
            Dead();
        }
    }
}
