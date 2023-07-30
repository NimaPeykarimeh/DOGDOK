using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject player;

    public bool isAlerted;
    [SerializeField] bool isPathOpen;

    [SerializeField] float maxMovementSpeed;
    [SerializeField] float minMovementSpeed;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationDuration;
    [SerializeField] float rotationTimer;

    [SerializeField] bool isRotating;
    [SerializeField] float rotateFreqDuration;
    [SerializeField] float rotateFreqTimer;
    [SerializeField] LayerMask obstacleLayer;

    Quaternion startingRotation;
    Quaternion rotationToLook;
    [SerializeField] Quaternion pivotRotation;
    [SerializeField] float currentAngle;
    [SerializeField] float targetRotation;
    [SerializeField] float directionDelta;
    public int rayCount = 8; // Number of rays to cast
    public float rayRange = 10f; // Length of the 

    // Start is called before the first frame update
    void Start()
    {

        movementSpeed = Random.Range(minMovementSpeed,maxMovementSpeed);
    }

    // Update is called once per frame
    void Update()
    {

        if (isAlerted)
        {
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

            characterController.Move(transform.forward * movementSpeed * Time.deltaTime);

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
        
        Vector3 directionToTarget = player.transform.position - transform.position;
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

            Ray ray = new Ray(transform.position, direction);
            Debug.DrawRay(ray.origin, ray.direction * rayRange, Color.red);

            // Perform actions based on raycast hits here
            if (Physics.Raycast(ray, out RaycastHit hit, rayRange, obstacleLayer))
            {
                isPathOpen = false;

                // You can check the hit object and do something based on the hit result
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
            }
            else
            {

                startingRotation = transform.rotation;
                pivotRotation = Quaternion.LookRotation(direction);
                rotationToLook = pivotRotation;
                rotationTimer = 0;
                isPathOpen = true;
                Debug.Log(i);
                return;
            }
        }
    }

}
