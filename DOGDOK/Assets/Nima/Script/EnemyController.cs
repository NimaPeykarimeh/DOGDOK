using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyMovement enemyMovement;
    public GameObject player;
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

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }
    private void OnEnable()//fix Later
    {
        isGroundedTimer = isGroundedLimit;
    }
    private void Update()
    {
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

    public bool IsGrounded(Transform center, float radius, LayerMask groundLayer)
    {
        Vector3 _center = center.position + new Vector3(0f, offset, 0f);// add offset from transform to vector

        return Physics.CheckSphere(_center, radius, groundLayer);
    }
}
