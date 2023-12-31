using UnityEngine;
using UnityEngine.UIElements;


public class EnemyController : MonoBehaviour
{
    public EnemyMovement enemyMovement;
    public EnemyHealth enemyHealth;
    public Animator animator;
    public EnemyFollow enemyFollow;
    public Transform player;
    public EnemySpawner enemySpawner;
    public AudioSource audioSource;
    public Collider enemyCollider;
    public Transform currentTargetTransform;
    public bool isTargetedTurret = false;
    public bool isAlerted;
    public bool isGrounded = false;
    [SerializeField] float isGroundedLimit = 5f;
    [SerializeField] float isGroundedTimer = 5f;
    [Header("Spehre Cast Values")]
    [SerializeField] float offset; 
    [SerializeField] Transform center;
    [SerializeField] float radius;
    [SerializeField] LayerMask groundLayer;
    [Header("Moving Around")]
    public Vector3 positionToGo;
    [SerializeField] float movingDistance = 5f;
    public bool isMoving;
    [SerializeField] float movingDuration = 10f;
    [SerializeField] float movingTimer;
    [Header("Audios")]
    [SerializeField] AudioClip alertedScream;
    [SerializeField] AudioClip voiceAlertedScream;
    [Header("Other")]
    [SerializeField] SkinnedMeshRenderer mesh;
    public Material material;
    public bool willAttackWall;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyFollow = GetComponent<EnemyFollow>();
        enemyCollider = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        enemyHealth = GetComponent<EnemyHealth>();
        currentTargetTransform = player.transform;
    }

    void Start()
    {
        material = mesh.material;
    }
    private void OnEnable()//fix Later
    {
        isGroundedTimer = isGroundedLimit;
        //enemyMovement.canMove = true;
    }

    public void ActivateRagdoll(bool isActive)
    {
        foreach (Rigidbody _rb in enemyHealth.bodyPartRb)
        {

            _rb.isKinematic = false;
            //_rb.GetComponent<Collider>().isTrigger = false;
            
        }
        enemyFollow.enabled = false;
        enemyMovement.enabled = false;
        animator.enabled = false;
    }

    private void Update()
    {
        if (!isAlerted)
        {
            if (!isMoving)
            {
                isMoving = true;
                movingTimer = movingDuration;

                //positionToGo = enemySpawner.GetRandomPositionInSpawner();
                float ranX = Random.Range(-movingDistance,movingDistance);
                float ranZ = Random.Range(-movingDistance, movingDistance);
                positionToGo = transform.position + new Vector3(ranX,0,ranZ);
                enemyFollow.positionToGo = positionToGo;
            }
            else
            {
                movingTimer -= Time.deltaTime;
                if (movingTimer <= 0 && enemyMovement.canMove)
                {
                    isMoving = false;
                }
            }
        }
        isGrounded = IsGrounded(center,radius,groundLayer);
        if (isGrounded)
        {
            isGroundedTimer = isGroundedLimit;
            enemyMovement.velocity.y = -0.2f;
        }
        if (!isGrounded)
        {
            isGroundedTimer -= Time.deltaTime;
            if (isGroundedTimer < 0)
            {
                enemySpawner.BackToPooler(transform);
            }
        }

    }

    public void StartRunning()
    {
        enemyMovement.SwitchMovmentState(EnemyMovement.MovementState.Runnning);
        //enemyMovement.canMove = true;
        animator.SetBool("IsAlerted", true);
    }

    void AlertScream(bool _isVoiceAlert)
    {
        float _randomPitch = Random.Range(0.85f,1.15f);

        audioSource.pitch = _randomPitch;
        if (_isVoiceAlert)
        {
            audioSource.PlayOneShot(voiceAlertedScream);
        }
        else
        {
            audioSource.PlayOneShot(alertedScream);
        }
    }

    public void AlertEnemy(bool _isAlerted,bool _voiceAlerted,bool _isTargetedTurret,Transform _targetTransform)//add a distance for zombie to be alerted if were too far
    {
        if (!isAlerted && _isAlerted)
        {
            float _wallAttackRandomizer = Random.Range(0f,1f);
            willAttackWall = _wallAttackRandomizer <= 0.3f;

            //enemyMovement.canMove = false;
            isTargetedTurret = _isTargetedTurret;
            currentTargetTransform = _targetTransform;
            positionToGo = _targetTransform.position;
            enemyFollow.positionToGo = _targetTransform.position;
            enemyMovement.SwitchMovmentState(EnemyMovement.MovementState.Idle);
            animator.SetLayerWeight(2,0f);
            if (_voiceAlerted)
            {
                float _randomizer = Random.Range(0f,1f);
                if (_randomizer < 0.4f)
                {
                    AlertScream(true);
                    animator.SetTrigger("VoiceAlerted");
                }
                else
                {
                    animator.SetTrigger("Alerted");
                    AlertScream(false);
                }
            }
            else
            {
                float _randomizer = Random.Range(0f, 1f);
                if (_randomizer < 0.4f)
                {
                    animator.SetTrigger("Alerted");
                    AlertScream(false);
                }
                else
                {
                    enemyMovement.SwitchMovmentState(EnemyMovement.MovementState.Runnning);
                    //enemyMovement.canMove = true;
                    //animator.SetBool("IsMoving", true);
                    animator.SetBool("IsAlerted", _isAlerted);
                }
            }
            //animator.SetBool("IsMoving", false);

        }
        else if ( !isTargetedTurret&& _isTargetedTurret && _isAlerted)
        {
            _targetTransform.GetComponent<TurretHealthManager>().alertedEnemiesList.Add(this);
            isTargetedTurret = _isTargetedTurret;
            currentTargetTransform = _targetTransform;
            positionToGo = _targetTransform.position;
            enemyFollow.positionToGo = _targetTransform.position;
        }
        
        isAlerted = _isAlerted;
        if (_isAlerted)
        {
            //enemyMovement.SwitchMovmentState(EnemyMovement.MovementState.Runnning);
            //enemyMovement.movementSpeed = enemyMovement.runSpeed;
            //enemyFollow.positionToGo = player.position;
            float _speedRatio = (enemyMovement.runSpeed - enemyMovement.minRunSpeed)/ (enemyMovement.maxRunSpeed - enemyMovement.minRunSpeed);
            animator.SetFloat("SpeedRatio", _speedRatio);
        }
        else
        {
            isTargetedTurret = false;
            willAttackWall = false;
            animator.SetBool("IsAlerted", _isAlerted);
            animator.SetLayerWeight(2, 1f);
            enemyMovement.SwitchMovmentState(EnemyMovement.MovementState.Walking);
            isTargetedTurret = false;
            //enemyMovement.movementSpeed = enemyMovement.walkSpeed;
            float _speedRatio = (enemyMovement.walkSpeed - enemyMovement.minWalkSpeed) / (enemyMovement.maxWalkSpeed- enemyMovement.minWalkSpeed);
            animator.SetFloat("SpeedRatio", _speedRatio);
        }
    }
    public bool IsGrounded(Transform center, float radius, LayerMask groundLayer)
    {
        Vector3 _center = center.position + new Vector3(0f, offset, 0f);// add offset from transform to vector

        return Physics.CheckSphere(_center, radius, groundLayer);
    }
}
