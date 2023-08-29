using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] float spawnInterval;
    [SerializeField] float spawnTimer;
    [SerializeField] GameObject enemyPrefab;
    EnemySpawnArea enemySpawnArea;
    [SerializeField] int maxEnemyCount;
    [SerializeField] bool isSpawning;

    [Header("Player Distance")]
    [SerializeField] GameObject player;
    [SerializeField] float playerDistance;
    [SerializeField] float maxPlayerDistance;
    [SerializeField] float minPlayerDistance;
    [SerializeField] int playerVisibleAngle = 90;
    [SerializeField] Transform playerOriantation;

    [Header("Timer")]
    [SerializeField] float distanceTimer;
    [SerializeField] float distanceInterval;
    [Header("Other")]
    [SerializeField] float dot;
    [SerializeField] float cosAngle;
    [Header("SphereCast")]
    [SerializeField] float sphereRadius;
    [SerializeField] LayerMask spawnCheckLayer;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawnArea = GetComponent<EnemySpawnArea>();
        //player = GameObject.FindGameObjectWithTag("Player");
        CreatePooler(maxEnemyCount);
    }

    public void BackToPooler(Transform _enemy)
    {
        _enemy.SetParent(transform);
        _enemy.gameObject.SetActive(false);
    }

    private void CreatePooler(int count)
    {
        for (int i = 0; i < count; i++)
        {
            
            GameObject _spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            _spawnedEnemy.transform.parent = transform;
            _spawnedEnemy.GetComponent<EnemyController>().enemySpawner = this;
            _spawnedEnemy.gameObject.SetActive(false);

        }
    }

    public Vector3 GetRandomPositionInSpawner()//organize this!!
    {
        int _tryLimit = 100;
        
        Vector3 _spawnArea = enemySpawnArea.spawnPosition;
        Vector3 _spawnSize = enemySpawnArea.spawnSize;
        
        for (int i = 0; i < _tryLimit; i++)
        {
            Transform _playerTransform = playerOriantation;

            Vector3 _forward = Quaternion.Euler(0f,playerOriantation.eulerAngles.y,0f) * Vector3.forward;

            float _x = Random.Range(_spawnArea.x - (_spawnSize.x / 2), _spawnArea.x + (_spawnSize.x / 2));
            float _y = _playerTransform.position.y;
            float _z = Random.Range(_spawnArea.z - (_spawnSize.z / 2), _spawnArea.z + (_spawnSize.z / 2));

            Vector3 _spawnPosition = new Vector3(_x, 0, _z);

            Vector3 playerPosition = new Vector3(_playerTransform.position.x,0, _playerTransform.position.z);

            Vector3 dirToPlayer = Vector3.Normalize(playerOriantation.position - _spawnPosition);

            dot = Vector3.Dot(_forward, dirToPlayer);
            cosAngle = -Mathf.Cos((playerVisibleAngle/2) * Mathf.Deg2Rad);
            float _distance = Vector3.Distance(playerPosition, _spawnPosition);
            if (dot > cosAngle && _distance > minPlayerDistance)
            {
                _spawnPosition.y = _spawnArea.y + _spawnSize.y / 2;//0;

                if (IsLocationEmpty(_spawnPosition) != Vector3.zero)
                {
                    return IsLocationEmpty(_spawnPosition);
                }
            }
        }
        //Debug.Log("couldn't spawn");

        return Vector3.zero;//if the height of the ground is different change it
    }

    Vector3 IsLocationEmpty(Vector3 _position)
    {
        RaycastHit hit;
        Ray ray = new Ray(_position,Vector3.down);

        if (Physics.SphereCast(ray,sphereRadius,out hit,30f, spawnCheckLayer))//maybe change 30 later depending on spawn height
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return hit.point;
            }
        }
        return Vector3.zero;
    }

    private void SpawnEnemy()
    {
        if (transform.childCount <= 0)
        {
            //Debug.Log("Max Enemy");
        }
        else
        {
            GameObject _spawnedEnemy = transform.GetChild(0).gameObject;
            Vector3 _randomPositon = GetRandomPositionInSpawner();
            if (_randomPositon != Vector3.zero)
            {
                _spawnedEnemy.transform.position = _randomPositon;
                _spawnedEnemy.SetActive(true);
                _spawnedEnemy.transform.parent = null;
                spawnTimer = 0;
            }
            else
            {
                //Debug.Log("Vector was Zero");
            }
        }
    }

    void CalculatePlayerDistance()//add one to each enemy
    {
        playerDistance = Vector3.Distance(transform.position,player.transform.position);
        isSpawning = playerDistance < maxPlayerDistance; //playerDistance < maxPlayerDistance && playerDistance > minPlayerDistance;
        distanceTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        distanceTimer += Time.deltaTime;
        if (distanceTimer >= distanceInterval)
        {
            CalculatePlayerDistance();
        }

        if (isSpawning)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
            }
        }
    }
}
