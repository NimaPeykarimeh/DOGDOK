using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyController enemyController;
    CharacterController characterController;
    public Rigidbody enemyRb;
    EnemyTeleportManager teleportManager;
    [SerializeField] MovementState currentMovementState;
    [Header("Walking")]
    public float walkSpeed;
    public float minWalkSpeed;
    public float maxWalkSpeed;

    [Header("Runnning")]
    public float runSpeed;
    public float minRunSpeed = 2.4f;
    public float maxRunSpeed = 3;
    public float movementSpeed;
    [SerializeField] float testVal;
    [Header("Changing Speed")]
    [SerializeField] float speedToMove;
    [SerializeField] bool isChangingSpeed = false;//D
    [SerializeField] float accelarationSpeed = 1f;
    public MovementState previousState;
    public bool canMove;
    public float gravity = -9.8f;
    public Vector3 velocity;

    [SerializeField]
    float _moveRandomizer;
    public enum MovementState
    {
        Idle,
        Crawl,
        Walking,
        Runnning
    }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        enemyRb = GetComponent<Rigidbody>();
        enemyController = GetComponent<EnemyController>();
        teleportManager = GetComponent<EnemyTeleportManager>();
        
    }

    public void ResetMovementState()
    {
        walkSpeed = Random.Range(minWalkSpeed, maxWalkSpeed);
        runSpeed = Random.Range(minRunSpeed, maxRunSpeed);
        movementSpeed = walkSpeed;
        float _speedRatio = (walkSpeed - minWalkSpeed) / (maxWalkSpeed - minWalkSpeed);

        _moveRandomizer = Random.Range(0f, 1f);
        if (_moveRandomizer < 0.6f)
        {
            SwitchMovmentState(MovementState.Walking,true);
        }
        else
        {
            SwitchMovmentState(MovementState.Idle,true);
        }
    }

    public void SwitchMovmentState(MovementState _state, bool isReseting = false)
    {
        isChangingSpeed = true;
        if (currentMovementState != MovementState.Crawl || isReseting)
        {
            if (_state == MovementState.Walking)
            {
                canMove = true;
                speedToMove = walkSpeed;
            }
            else if (_state == MovementState.Crawl)
            {
                speedToMove = walkSpeed;
                canMove = true;
            }
            else if (_state == MovementState.Runnning)
            {
                canMove = true;
                speedToMove = runSpeed;
            }

            else if (_state == MovementState.Idle)
            {
                canMove = false;
                speedToMove = 0;
            }
            previousState = currentMovementState;
            currentMovementState = _state;
        }
        
        
        //enemyController.animator.SetBool("IsMoving", canMove);
    }

    void Start()
    {
        walkSpeed = Random.Range(minWalkSpeed, maxWalkSpeed);
        runSpeed = Random.Range(minRunSpeed, maxRunSpeed);
        movementSpeed = walkSpeed;
        float _speedRatio = (walkSpeed - minWalkSpeed) / (maxWalkSpeed - minWalkSpeed);

        _moveRandomizer = Random.Range(0f,1f);
        if (_moveRandomizer < 0.6f)
        {
            SwitchMovmentState(MovementState.Walking);
        }
        else
        {
            SwitchMovmentState(MovementState.Idle);
        }
        
        enemyController.animator.SetFloat("SpeedRatio", _speedRatio);
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

        enemyController.animator.SetBool("IsMoving", movementSpeed > 0.1f);

    }
    void Update()
    {
        //if (canMove)
        //{
        //    if (enemyController.isAlerted || enemyController.isMoving)
        //    {
        //        Move();
        //        //characterController.Move(transform.forward * movementSpeed * Time.deltaTime);
        //    }

        //    //velocity.y += gravity * Time.deltaTime;
        //    //characterController.Move(velocity * Time.deltaTime);
        //}
        Move();
        if (isChangingSpeed)
        {
            movementSpeed = Mathf.MoveTowards(movementSpeed,speedToMove,(1/accelarationSpeed) * Time.deltaTime);
            if (movementSpeed == speedToMove)
            {
                isChangingSpeed = false;
            }
        }
        //else
        //{
        //    enemyRb.velocity = Vector3.zero;//optimize et
        //}
        
    }
}
