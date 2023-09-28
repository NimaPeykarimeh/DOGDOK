using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class EnemyFollow : MonoBehaviour
{
    
    [SerializeField] EnemyController enemyController;

    [Header("Zombie eyes")]
    [SerializeField] LayerMask visibleLayers;
    [SerializeField] Transform eyes;
    [SerializeField] float eyeSphereRadius = 0.5f;
    [SerializeField] int visualAngleLimit = 90;
    [SerializeField] float cosValue;
    [SerializeField] bool isInAngle;
    [SerializeField] int alertedSeeDisntance = 20;
    [SerializeField] int defaultSeeDistance = 10;
    [SerializeField] int seeDistance = 10;
    [SerializeField] bool isInDistance;
    [SerializeField] float outOfEyeDelay = 0.5f;
    [SerializeField] float outOfEyeTimer;
    bool isInVision;
    public Vector3 playerLastKnowPosition;

    [Header("Rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] float rotationTimer;

    [SerializeField] bool isRotating;
    [SerializeField] float rotateFreqDuration;
    [SerializeField] float rotateFreqTimer;

    [Header("other")]
    [SerializeField] float _dis;
    public Vector3 positionToGo;
    [SerializeField] float reachTolerance = 1f;
    [SerializeField] bool isPlayerPositionKnown;
    [SerializeField] LayerMask obstacleLayer;

    Quaternion startingRotation;
    Quaternion rotationToLook;
    [SerializeField] Quaternion pivotRotation;
    [SerializeField] float currentAngle;
    [SerializeField] float targetRotation;
    [SerializeField] float directionDelta;
    [SerializeField] Transform rayCastCenter;
    public int rayCount = 8; // Number of rays to cast
    public float rayRange = 10f; // Length of the 

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    void GetPlayerDirection()
    {
        Vector3 dirToTarget = Vector3.Normalize(enemyController.player.position - transform.position);
        float dot = Vector3.Dot(transform.forward, dirToTarget);
        cosValue = Mathf.Cos((visualAngleLimit / 2) * Mathf.Deg2Rad);
        isInAngle = (dot >= cosValue);
        
    }


    void GetPlayerDistance()
    {
        float _distance = Vector3.Distance(enemyController.player.position,transform.position);
        if (enemyController.isAlerted)
        {
            seeDistance = alertedSeeDisntance;
        }
        else
        {
            seeDistance = defaultSeeDistance;
        }
        isInDistance = _distance <= seeDistance;
        if (isInDistance)
        {
            seeDistance = Mathf.RoundToInt(_distance + 1f);
        }
    }

    void DetectThePlayer()
    {
        
        if (isInDistance && isInAngle)//!enemyController.isAlerted
        {
            Vector3 _direction = (enemyController.player.position - eyes.position).normalized;
            _direction.y = 0;
            Ray ray = new Ray(eyes.position, _direction);
            RaycastHit hit;

            if (Physics.SphereCast(ray, eyeSphereRadius,out hit, seeDistance, visibleLayers))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                if (hit.transform.CompareTag("Player"))
                {
                    if (!enemyController.isAlerted)
                    {
                        enemyController.AlertEnemy(true,false,false,hit.transform);
                    }
                    outOfEyeTimer = outOfEyeDelay;
                    isInVision = true;
                    isPlayerPositionKnown = true;
                    playerLastKnowPosition = hit.transform.position;
                    positionToGo = playerLastKnowPosition;

                }
                else if(isInVision)
                {
                    isInVision = false;
                    outOfEyeTimer = outOfEyeDelay;
                }
            }
            else if(isInVision)
            {
                isInVision = false;
                outOfEyeTimer = outOfEyeDelay;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyController.isTargetedTurret)
        {
        GetPlayerDirection();
        GetPlayerDistance();
        DetectThePlayer();

        }

        if (!isInVision && enemyController.isAlerted && !enemyController.isTargetedTurret)
        {
            outOfEyeTimer -= Time.deltaTime;

            if (outOfEyeTimer > 0)
            {
                playerLastKnowPosition = enemyController.player.transform.position;
                positionToGo = playerLastKnowPosition;
            }
            else
            {
                isPlayerPositionKnown = false;
            }
            if (!isPlayerPositionKnown)
            {
                _dis = Vector3.Distance(transform.position, positionToGo);
                if (_dis <= reachTolerance && !isInDistance)
                {
                    enemyController.AlertEnemy(false,false,false,transform);
                }
            }
        }

        

        else if (enemyController.isMoving && !enemyController.isAlerted && !enemyController.isTargetedTurret)
        {
            //positionToGo = enemyController.player.position;
            _dis = Vector3.Distance(transform.position, positionToGo);
            if (_dis <= reachTolerance)
            {
                enemyController.isMoving = false;
            }

        }
        rotateFreqTimer += Time.deltaTime;
        if (rotateFreqTimer >= rotateFreqDuration)
        {
            isRotating = true;
            GetNewDirection();
            rotateFreqTimer = 0;
        }
        if (isRotating)
        {
            rotationTimer += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startingRotation, rotationToLook, rotationTimer / rotationDuration);
            if (rotationTimer >= rotationDuration)
            {
                isRotating = false;

            }
        }
    }
    int NormalizeAngle(float _angle)
    {
        if (_angle > 180)
        {
            _angle -= 360;
        }
        else if (_angle < -180)
        {
            _angle += 360;
        }

        if (_angle > 0)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    void GetNewDirection()
    {
        
        Vector3 directionToTarget = positionToGo - transform.position;
        directionToTarget.y = 0;

        pivotRotation = Quaternion.LookRotation(directionToTarget);
        targetRotation = pivotRotation.eulerAngles.y;
        currentAngle = transform.rotation.eulerAngles.y;
        directionDelta = currentAngle - targetRotation;

        int _rotationDirection = NormalizeAngle(directionDelta);
        //Quaternion castRotation;

        for (int i = 0; i < rayCount; i++)
        {

            float angle = (i * (360f / rayCount)) * _rotationDirection; //
            //if (i>0)
            //{
            //   angle -= (i - 1) * (360f / rayCount);
            //}

            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 direction = rotation * pivotRotation * Vector3.forward;

            Ray ray = new Ray(rayCastCenter.position, direction);
            Debug.DrawRay(ray.origin, ray.direction * rayRange, Color.red);

            bool _doesHit = Physics.Raycast(ray, out RaycastHit hit, rayRange, obstacleLayer);

            // Perform actions based on raycast hits here
            if (!_doesHit)
            {
                startingRotation = transform.rotation;
                pivotRotation = Quaternion.LookRotation(direction);
                rotationToLook = pivotRotation;
                rotationTimer = 0;
                return;
            }
            else
            {
                if (enemyController.willAttackWall && hit.collider.CompareTag("Wall"))
                {
                    startingRotation = transform.rotation;
                    pivotRotation = Quaternion.LookRotation(direction);
                    rotationToLook = pivotRotation;
                    rotationTimer = 0;
                    return;
                }
            }
        }
    }

}
