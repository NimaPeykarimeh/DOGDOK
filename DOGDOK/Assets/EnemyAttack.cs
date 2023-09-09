using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyController enemyController;
    EnemyMovement enemyMovement;
    [SerializeField] int damage = 5;
    float attackTimer;
    [SerializeField] float attackInterval = 2;
    [SerializeField] float attackDistance = 1;

    [SerializeField] float stopDelay = 0.3f;
    bool readyToAttack;
    [Header("AnimationDelay")]
    [SerializeField] float animationTimer = 0f;
    [SerializeField] float animationDelay = 0.3f;
    [SerializeField] bool isDelaying = false;
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    void Attack()
    {
        if (readyToAttack)
        {
            enemyController.player.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }
        //enemyMovement.canMove = true;
        isDelaying = true;
        animationTimer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        readyToAttack = Vector3.Distance(transform.position, enemyController.player.position) < attackDistance && enemyController.isAlerted;

        if (attackTimer >= attackInterval - stopDelay && readyToAttack)
        {
            enemyMovement.canMove = false;
        }

        if (attackTimer >= attackInterval && readyToAttack)
        {
            attackTimer = 0;
            enemyController.animator.SetTrigger("Attack");
            //Attack();
        }
        if (isDelaying)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= animationDelay)
            {
                enemyMovement.canMove = true;
                isDelaying = false;
            }
        }
        
    }
}
