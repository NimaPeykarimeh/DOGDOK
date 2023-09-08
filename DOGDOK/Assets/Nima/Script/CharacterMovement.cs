using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NoiseMaker))]
public class CharacterMovement : MonoBehaviour
{
    PlayerController playerController;
    Rigidbody rb;
    [SerializeField] GameObject playerObject;

    [Header("Movement")]
    float moveX;
    float moveZ;
    Vector3 playerInput;
    [SerializeField] float inputMagnitude;
    [SerializeField] Vector3 move;
    MoveStates currentMoveState;
    [Header("Speeds")]
    public float currentVelocity;
    public float speedToMove;
    public float currentMovementSpeed;
    public float walkingSpeed;
    public float runningSpeed;
    public float crouchingSpeed;
    public float turnSpeed = 10f;
    public bool isRunning;

    [Header("Stamina")]
    [SerializeField] float maxStamina;
    [SerializeField] float currentStamina;
    [SerializeField] Image staminaImage;
    [SerializeField] float staminaRegeneration;
    [SerializeField] float staminaUsage;
    [Header("Acceleration")]

    [SerializeField] float accelerationDuration = 1f;
    [SerializeField] float accelerationTimer;
    [SerializeField] float decelerationDuration;
    [SerializeField] float accelerationSpeed;
    [SerializeField] float decelerationSpeed;
    float firstSpeed;
    float lastSpeed;
    bool isAccelerating;

    [Space]

    public  CharacterController characterController;
    public Vector3 mousePosition;
    [Header("Noise")]
    NoiseMaker noiseMaker;
    [SerializeField] float noiseMult;
    [SerializeField] Transform noiseCenter;

    public enum MoveStates
    {
        Run,
        Walk,
        Crouched
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        noiseMaker = GetComponent<NoiseMaker>();
    }

    void MovePlayer()
    {
        if (playerController.currentState == PlayerController.PlayerStates.Basic)
        {
            //characterController.Move(move * currentVelocity * Time.deltaTime);
            rb.velocity = new Vector3((move * currentVelocity).x,rb.velocity.y ,(move * currentVelocity).z);
            if (!playerController.isAiming && move.magnitude > 0)
            {
                RotateTowards(move);
            }
        }
        else
        {
            //characterController.Move(move * currentVelocity * Time.deltaTime);
            rb.velocity = new Vector3((move * currentVelocity).x, rb.velocity.y, (move * currentVelocity).z);
            RotateTowards(playerController.mainCamera.transform.forward);
        }
    }
    //
    private void RotateTowards(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        targetRotation.x = 0;
        targetRotation.z = 0;
        playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
    }

    public void ToggleRunState(MoveStates _state)
    {
        currentMoveState = _state;
        if (_state == MoveStates.Walk)
        {
            playerController.isWalking = true;
            playerController.isRunning = false;
            playerController.isCrouching = false;
            //movementSpeed = walkSpeed;
            //isAccelerating = true;

            currentMovementSpeed = walkingSpeed;
        }
        else if(_state == MoveStates.Run && playerController.canRun)
        {
            playerController.isWalking = false;
            playerController.isRunning = true;
            playerController.isCrouching = false;
            //isAccelerating = true;

            currentMovementSpeed = runningSpeed;
        }
        else if (_state == MoveStates.Crouched)
        {
            playerController.isWalking = false;
            playerController.isRunning = false;
            playerController.isCrouching = true;

            currentMovementSpeed = crouchingSpeed;
        }
        playerController.animator.SetBool("IsCrouching", _state == MoveStates.Crouched);
    }

    void Accelarate()
    {
        isAccelerating = true;
        firstSpeed = currentVelocity;

    }

    void GetMovementDirection()
    {
        
        //get The forward and right direction of camera
        Vector3 cameraForward = playerController.mainCamera.transform.forward;
        Vector3 cameraRight = playerController.mainCamera.transform.right;

        //reset the one to prevent vertical rotation
        cameraRight.y = 0;
        cameraForward.y = 0;

        move = (cameraRight * moveX + cameraForward * moveZ).normalized;
        //return move.magnitude;
    }

    void GetPlayerInput()
    {
        //get Player Input
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        playerInput = new Vector3(moveX,0f,moveZ).normalized;
        inputMagnitude = playerInput.magnitude;
    }

    void UpdateStamina()
    {
        if (playerController.isRunning)
        {
            currentStamina -= staminaUsage * Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                ToggleRunState(MoveStates.Walk);
            }
            staminaImage.fillAmount = currentStamina / maxStamina;
        }
        else if(!playerController.isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRegeneration * Time.deltaTime;
            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
            }
            staminaImage.fillAmount = currentStamina / maxStamina;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    void Update()
    {
        
        //currentSpeed = GetMovement() * currentMovementSpeed;
        GetPlayerInput();
        if (inputMagnitude > 0)
        {
            playerController.animator.SetFloat("DirX",moveX, 0.1f, Time.deltaTime);
            playerController.animator.SetFloat("DirY", moveZ,0.1f, Time.deltaTime);
            GetMovementDirection();
            //currentVelocity = inputMagnitude * currentMovementSpeed;
        }
        if (currentVelocity > 0 )
        {
            noiseMaker.MakeNoise(noiseMult * currentVelocity, noiseCenter);
        }
        speedToMove = inputMagnitude * currentMovementSpeed;
        UpdateStamina();
        

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleRunState(MoveStates.Run);
            isRunning = true;//to make the player run after aim
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            if (playerController.isCrouching)
            {
                ToggleRunState(MoveStates.Crouched);
            }
            else
            {
                ToggleRunState(MoveStates.Walk);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleRunState(MoveStates.Crouched);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (playerController.aimPlayer.isAiming)
            {
                ToggleRunState(MoveStates.Crouched);
            }
            else
            {
                ToggleRunState(MoveStates.Walk);
            }
        }

        Accelarate();
        if (isAccelerating)
        {
            if (currentVelocity < speedToMove)
            {
                //accelerationTimer = Mathf.Clamp(accelerationTimer + (Time.deltaTime * accelerationSpeed), 0, accelerationDuration);
                currentVelocity = Mathf.MoveTowards(currentVelocity, speedToMove, accelerationSpeed * Time.deltaTime);
            }
            else
            {
                //accelerationTimer = Mathf.Clamp(accelerationTimer - (Time.deltaTime * decelerationSpeed), 0, accelerationDuration);
                currentVelocity = Mathf.MoveTowards(currentVelocity, speedToMove, decelerationSpeed * Time.deltaTime);
            }
            //accelerationTimer += Time.deltaTime;
            //currentMovementSpeed = Mathf.Lerp(firstSpeed,lastSpeed,accelerationTimer/accelerationDuration);
            //if (accelerationTimer > accelerationDuration)
            //{
            //    isAccelerating = false;
            //    accelerationTimer = 0;
            //    currentMovementSpeed = lastSpeed;
            //}
        }

    }


}
