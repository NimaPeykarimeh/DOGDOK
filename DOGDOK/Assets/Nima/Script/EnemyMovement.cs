using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyController enemyController;
    CharacterController characterController;
    public Rigidbody enemyRb;
    EnemyTeleportManager teleportManager;
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float minMovementSpeed;
    [SerializeField] float movementSpeed;
    [SerializeField] float testVal;
    public bool canMove;
    public float gravity = -9.8f;
    public Vector3 velocity;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        enemyRb = GetComponent<Rigidbody>();
        enemyController = GetComponent<EnemyController>();
        teleportManager = GetComponent<EnemyTeleportManager>();
        movementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
        testVal = Random.Range(0f,1f);
        enemyController.animator.SetFloat("MovementSpeed",testVal);
    }
    private void OnEnable()//fix later for organizing
    {
        velocity.y = -0.2f;
    }
    // Update is called once per frame
    void Move()
    {
        enemyRb.velocity = transform.forward * movementSpeed;
    }
    void Update()
    {
        if (canMove)
        {
            if (enemyController.isAlerted || enemyController.isMoving)
            {
                Move();
                //characterController.Move(transform.forward * movementSpeed * Time.deltaTime);
            }

            //velocity.y += gravity * Time.deltaTime;
            //characterController.Move(velocity * Time.deltaTime);
        }
    }
}
