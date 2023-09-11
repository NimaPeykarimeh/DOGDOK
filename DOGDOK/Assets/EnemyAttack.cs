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
    [SerializeField] float hitDistance = 1;
    [SerializeField] float attackTriggerDistance = 1.5f;

    [SerializeField] float stopDelay = 0.3f;
    bool readyToAttack;
    [Header("AnimationDelay")]
    [SerializeField] float animationTimer = 0f;
    [SerializeField] float animationDelay = 0.3f;
    [SerializeField] bool isDelaying = false;
    [Header("Animation layer")]
    bool isLayerChanging;
    int attackLayer = 1;
    float layerTargetValue;
    [SerializeField] float layerChangeDuration = 0.2f;
    [SerializeField] float layerValue;
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    void Attack()
    {
        bool isInAttackDistance = Vector3.Distance(transform.position, enemyController.player.position) < hitDistance;
        Debug.Log("AttackCounter");
        isLayerChanging = true;
        layerValue = 1;
        layerTargetValue = 0;
        layerChangeDuration = 0.7f;
        if (isInAttackDistance)
        {
            enemyController.player.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }
        //enemyMovement.canMove = true;
        isDelaying = true;
        animationTimer = 0;
    }


    void SetRandomAttackValue()
    {
        float _randomizer = Random.Range(0,3.9f);
        float _flooredRandom = Mathf.FloorToInt(_randomizer);
        float _randomValue = _flooredRandom / 3;
        Debug.Log(_randomValue);
        enemyController.animator.SetFloat("AttackRandomizer",_randomValue);

    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        readyToAttack = Vector3.Distance(transform.position, enemyController.player.position) < attackTriggerDistance && enemyController.isAlerted;

        //if (attackTimer >= attackInterval - stopDelay && readyToAttack)
        //{
        //    enemyMovement.canMove = false;
        //}

        if (attackTimer >= attackInterval && readyToAttack)
        {
            attackTimer = 0;
            SetRandomAttackValue();
            enemyController.animator.SetTrigger("Attack");
            Debug.Log("TriggerCounter");
            layerValue = 0;
            isLayerChanging = true;
            layerTargetValue = 1;
            layerChangeDuration = 0.2f;
            //Attack();
        }

        if (isLayerChanging)
        {
            layerValue = Mathf.MoveTowards(layerValue, layerTargetValue, (1/layerChangeDuration) * Time.deltaTime);
            enemyController.animator.SetLayerWeight(1, layerValue);
            if (layerValue == layerTargetValue)
            {
                isLayerChanging = false;
            }
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
