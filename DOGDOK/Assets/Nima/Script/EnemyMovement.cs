using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyController enemyController;
    CharacterController characterController;
    public Rigidbody enemyRb;
    EnemyTeleportManager teleportManager;
    [Header("Walking")]
    public float walkSpeed;
    public float maxWalkSpeed;
    public float minWalkSpeed;

    [Header("Runnning")]
    public float runSpeed;
    public float minRunSpeed = 2.4f;
    public float maxRunSpeed = 3;

    public float movementSpeed;
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
        walkSpeed = Random.Range(minWalkSpeed, maxWalkSpeed);
        runSpeed = Random.Range(minRunSpeed, maxRunSpeed);
        movementSpeed = walkSpeed;

        float _speedRatio = (walkSpeed - minWalkSpeed) / (maxWalkSpeed - minWalkSpeed);
        enemyController.animator.SetFloat("MovementSpeed", _speedRatio);
        //enemyController.animator.SetFloat("MovementSpeed",s);
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
        else
        {
            enemyRb.velocity = Vector3.zero;//optimize et
        }
    }
}
