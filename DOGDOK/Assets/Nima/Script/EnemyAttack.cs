using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyController enemyController;
    EnemyMovement enemyMovement;
    EnemyHealth enemyHealth;
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
    float randomAttackValue;
    bool isLayerChanging;
    int attackLayer = 1;
    float layerTargetValue;
    [SerializeField] float layerChangeDuration = 0.2f;
    [SerializeField] float layerValue;
    [Header("TurretAttack")]
    [SerializeField] bool isTargetedTurret;
    [SerializeField] float turretDetectionRange = 15f;
    [SerializeField] List<GameObject> turretObjectList;
    [SerializeField] float turretCheckInterval = 0.5f;
    [SerializeField] float turretCheckTimer;
    [SerializeField] TurretHealthManager turretHealthManager;

    [Header("Test")]
    [SerializeField] float targetDistance;
    [SerializeField] float stopDistance = 0.75f;
    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyHealth = GetComponent<EnemyHealth>();
    }
    void Attack()
    {
        bool isInAttackDistance = Vector3.Distance(transform.position, enemyController.currentTargetTransform.position) < hitDistance;
        isLayerChanging = true;
        layerValue = 1;
        layerTargetValue = 0;
        layerChangeDuration = 0.7f;
        if (isInAttackDistance)
        {
            if (enemyController.isTargetedTurret)
            {
                turretHealthManager.GetDamage(damage);
            }
            else
            {
                if ((randomAttackValue < 0.5f && enemyHealth.itHasLeftHand) || (randomAttackValue > 0.5f && enemyHealth.itHasRightHand))
                {
                    enemyController.player.GetComponent<PlayerHealth>().ChangeHealth(-damage);
                }
            }
        }
        //enemyMovement.canMove = true;
        isDelaying = true;
        animationTimer = 0;
    }

    void CheckNearbyTurrets()
    {
        if (true)
        {
            turretObjectList.Clear();
            Collider[] _hitColliders = Physics.OverlapSphere(transform.position,turretDetectionRange);
            foreach (Collider _collider in _hitColliders)
            {
                if (_collider.CompareTag("Turret"))
                {
                    turretObjectList.Add(_collider.gameObject);
                }
            }
            if (turretObjectList.Count>0)
            {
                float _minValue = 100;
                GameObject _selectedTurret = null;
                foreach (GameObject _turretObject in turretObjectList)
                {
                    float _distance = Vector3.Distance(transform.position,_turretObject.transform.position);
                    if (_distance < _minValue)
                    {
                        _selectedTurret = _turretObject;
                        _minValue = _distance;
                    }
                }
                turretHealthManager = _selectedTurret.GetComponent<TurretHealthManager>();
                enemyController.enemyFollow.positionToGo = _selectedTurret.transform.position;
                enemyController.currentTargetTransform = _selectedTurret.transform;
                turretCheckTimer = turretCheckInterval;
                isTargetedTurret = true;
            }
        }
    }

    void SetRandomAttackValue()//get random value to attacking hand
    {
        float _randomizer = Random.Range(0,3.9f);
        float _flooredRandom = Mathf.FloorToInt(_randomizer);
        randomAttackValue = _flooredRandom / 3;
        enemyController.animator.SetFloat("AttackRandomizer", randomAttackValue);

    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        targetDistance = Vector3.Distance(transform.position, enemyController.currentTargetTransform.position);
        readyToAttack = targetDistance < attackTriggerDistance && enemyController.isAlerted;

        turretCheckTimer -= Time.deltaTime;
        if (turretCheckTimer <= 0)
        {
            CheckNearbyTurrets();
        }

        if (readyToAttack)
        {
            if (targetDistance <= stopDistance)
            {
                enemyMovement.SwitchMovmentState(EnemyMovement.MovementState.Idle);
            }
            else
            {
                enemyMovement.SwitchMovmentState(EnemyMovement.MovementState.Runnning);
            }
            //enemyMovement.canMove = playerDistance >= stopDistance;

        }
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
                isDelaying = false;
            }
        }
        
    }
}
