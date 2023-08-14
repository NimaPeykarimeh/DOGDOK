using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class EnemyHealth : MonoBehaviour
{
    EnemyController enemyController;
    [SerializeField] int headShotMult;
    [SerializeField] int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        currentHealth = maxHealth;
    }

    private void Dead()
    {
        enemyController.enemySpawner.BackToPooler(transform);
        currentHealth = maxHealth;// change later
    }

    public void GetDamage(int _damage,EnemyBodyPartDamageDetection.BodyParts _bodyPart)
    {
        if (_bodyPart == EnemyBodyPartDamageDetection.BodyParts.Head)
        {
            _damage *= headShotMult;
        }
        enemyController.AlertEnemy();
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
            Dead();
        }
    }
}
