using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    EnemyMovement enemyMovement;
    EnemyFollow enemyFollow;
    public Transform player;
    public EnemySpawner enemySpawner;
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
    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyFollow = GetComponent<EnemyFollow>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnEnable()//fix Later
    {
        isGroundedTimer = isGroundedLimit;
    }
    private void Update()
    {
        if (!isAlerted)
        {
            if (!isMoving)
            {
                isMoving = true;
                movingTimer = movingDuration;

                positionToGo = enemySpawner.GetRandomPositionInSpawner();
            }
            else
            {
                movingTimer -= Time.deltaTime;
                if (movingTimer <= 0)
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

    public void AlertPlayer(Vector3 _playerPosition)
    {
        isAlerted = true;
        enemyFollow.positionToGo = _playerPosition;
    }
    public bool IsGrounded(Transform center, float radius, LayerMask groundLayer)
    {
        Vector3 _center = center.position + new Vector3(0f, offset, 0f);// add offset from transform to vector

        return Physics.CheckSphere(_center, radius, groundLayer);
    }
}
