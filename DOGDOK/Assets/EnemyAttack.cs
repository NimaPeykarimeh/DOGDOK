using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyController enemyController;

    [SerializeField] int damage = 5;
    float attackTimer;
    [SerializeField] float attackInterval = 2;
    [SerializeField] float attackDistance = 1;
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        
    }
    void Attack()
    {
        enemyController.player.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        
        attackTimer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval && Vector3.Distance(transform.position,enemyController.player.position) < attackDistance && enemyController.isAlerted)
        {
            enemyController.animator.SetTrigger("Attack");
            //Attack();
        }
    }
}
